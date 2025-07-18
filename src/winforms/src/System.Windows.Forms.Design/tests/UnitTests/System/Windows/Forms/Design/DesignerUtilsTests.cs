﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms.Design.Behavior;
using Moq;

namespace System.Windows.Forms.Design.Tests;

public class DesignerUtilsTests :IDisposable
{
    private readonly Bitmap _bitmap;
    private readonly Graphics _graphics;
    private readonly Rectangle _bounds;

    public Button Button { get; }

    public DesignerUtilsTests()
    {
        _bitmap = new(20, 20);
        _graphics = Graphics.FromImage(_bitmap);
        _bounds = new(5, 5, 20, 20);
        Button = new() { Width = 50, Height = 50 };
    }

    public void Dispose()
    {
        _graphics.Dispose();
        _bitmap.Dispose();
        Button.Dispose();
    }

    [WinFormsFact]
    public void BoxImage_ShouldReturnNonNullImage() =>
        DesignerUtils.BoxImage.Should().BeOfType<Bitmap>();

    [WinFormsFact]
    public void BoxImage_ShouldHaveExpectedDimensions()
    {
        DesignerUtils.BoxImage.Width.Should().Be(DesignerUtils.s_boxImageSize);
        DesignerUtils.BoxImage.Height.Should().Be(DesignerUtils.s_boxImageSize);
    }

    [WinFormsFact]
    public void BoxImage_ShouldHaveExpectedPixelFormat() =>
        ((Bitmap)DesignerUtils.BoxImage).PixelFormat.Should().Be(PixelFormat.Format32bppPArgb);

    [WinFormsFact]
    public void BoxImage_ShouldBeCached()
    {
        Image firstCall = DesignerUtils.BoxImage;
        Image secondCall = DesignerUtils.BoxImage;
        firstCall.Should().BeSameAs(secondCall);
    }

    [Fact]
    public void HoverBrush_ShouldBeSolidBrush() =>
        DesignerUtils.HoverBrush.Should().BeOfType<SolidBrush>();

    [Fact]
    public void HoverBrush_ShouldHaveExpectedColor() =>
        ((SolidBrush)DesignerUtils.HoverBrush).Color.Should().Be(Color.FromArgb(50, SystemColors.Highlight));

    [Fact]
    public void MinDragSize_ShouldReturnNonEmptySize()
    {
        Size minDragSize = DesignerUtils.MinDragSize;
        minDragSize.Should().NotBe(Size.Empty);
    }

    [Fact]
    public void MinDragSize_ShouldBeCached()
    {
        Size firstCall = DesignerUtils.MinDragSize;
        Size secondCall = DesignerUtils.MinDragSize;
        firstCall.Should().Be(secondCall);
    }

    [WinFormsTheory]
    [MemberData(nameof(ResizeBorderTestData))]
    public void DrawResizeBorder_ShouldUseCorrectBrushBasedOnBackColor(Color backColor, Color expectedColor)
    {
        using Region region = new(new Rectangle(0, 0, _bounds.Width, _bounds.Height));
        DesignerUtils.DrawResizeBorder(_graphics, region, backColor);
        Color pixelColor = _bitmap.GetPixel(_bounds.Width / 2, _bounds.Height / 2);
        pixelColor.ToArgb().Should().Be(expectedColor.ToArgb());
    }

    public static TheoryData<Color, Color> ResizeBorderTestData => new()
    {
        { Color.White, SystemColors.ControlDarkDark },
        { Color.Black, SystemColors.ControlLight }
    };

    [WinFormsFact]
    public void DrawGrabHandle_ShouldNotThrow_WhenCalledWithValidParameters()
    {
        Exception? exception = Record.Exception(() => DesignerUtils.DrawGrabHandle(_graphics, _bounds, isPrimary: true));
        exception.Should().BeNull();
    }

    [WinFormsTheory]
    [BoolData]
    public void DrawGrabHandle_ShouldDrawHandle_BasedOnSelectionType(bool isPrimary)
    {
        DesignerUtils.DrawGrabHandle(_graphics, _bounds, isPrimary);

        Color pixelColor = _bitmap.GetPixel(6, 6);
        pixelColor.ToArgb().Should().NotBe(Color.Empty.ToArgb());
    }

    [WinFormsTheory]
    [BoolData]
    public void DrawLockedHandle_ShouldDrawHandle_BasedOnSelectionType(bool isPrimary)
    {
        DesignerUtils.DrawLockedHandle(_graphics, _bounds, isPrimary);

        Color pixelColor = _bitmap.GetPixel(6, 6);
        pixelColor.ToArgb().Should().NotBe(Color.Empty.ToArgb());
    }

    [WinFormsFact]
    public void DrawLockedHandle_ShouldDrawUpperRect_WhenCalledWithPrimarySelection()
    {
        DesignerUtils.DrawLockedHandle(_graphics, _bounds, isPrimary: true);
        Color pixelColor = _bitmap.GetPixel(_bounds.Left + 1, _bounds.Top + 1);
        pixelColor.ToArgb().Should().NotBe(Color.Empty.ToArgb());
    }

    [WinFormsFact]
    public void DrawLockedHandle_ShouldDrawLowerRect_WhenCalledWithNonPrimarySelection()
    {
        DesignerUtils.DrawLockedHandle(_graphics, _bounds, isPrimary: false);
        Color pixelColor = _bitmap.GetPixel(_bounds.Left + 1, _bounds.Top + DesignerUtils.s_lockedHandleLowerOffset + 1);
        pixelColor.ToArgb().Should().NotBe(Color.Empty.ToArgb());
    }

    [WinFormsFact]
    public void DrawSelectionBorder_ShouldNotThrow_WhenCalledWithValidParameters()
    {
        Exception? exception = Record.Exception(() => DesignerUtils.DrawSelectionBorder(_graphics, _bounds));
        exception.Should().BeNull();
    }

    [WinFormsFact]
    public void DrawSelectionBorder_ShouldFillRectangle_WhenCalled()
    {
        DesignerUtils.DrawSelectionBorder(_graphics, _bounds);
        Color pixelColor = _bitmap.GetPixel(6, 6);
        pixelColor.ToArgb().Should().NotBe(Color.Empty.ToArgb());
    }

    [WinFormsFact]
    public void LastCursorPoint_ShouldNotThrow_WhenCalled()
    {
        Exception? exception = Record.Exception(() =>
            _ = DesignerUtils.LastCursorPoint);

        exception.Should().BeNull();
    }

    [WinFormsFact]
    public void LastCursorPoint_ShouldReturnValidCoordinates()
    {
        Point cursorPoint = DesignerUtils.LastCursorPoint;

        cursorPoint.X.Should().BeLessThanOrEqualTo(SystemInformation.VirtualScreen.Right);
        cursorPoint.X.Should().BeGreaterThanOrEqualTo(SystemInformation.VirtualScreen.Left);
        cursorPoint.Y.Should().BeLessThanOrEqualTo(SystemInformation.VirtualScreen.Bottom);
        cursorPoint.Y.Should().BeGreaterThanOrEqualTo(SystemInformation.VirtualScreen.Top);
    }

    [WinFormsFact]
    public void SyncBrushes_ShouldRecreateHoverBrush()
    {
        DesignerUtils.SyncBrushes();

        DesignerUtils.HoverBrush.Should().BeOfType<SolidBrush>();
        ((SolidBrush)DesignerUtils.HoverBrush).Color.Should().Be(Color.FromArgb(50, SystemColors.Highlight));
    }

    [WinFormsTheory]
    [InlineData(FrameStyle.Dashed, true)]
    [InlineData(FrameStyle.Thick, false)]
    public void DrawFrame_ShouldUseCorrectBrushBasedOnStyleAndBackColor(FrameStyle style, bool useDarkBackground)
    {
        using Region region = new(new Rectangle(0, 0, _bounds.Width, _bounds.Height));
        Color backColor = useDarkBackground ? Color.Black : Color.White;
        Color expectedColor = useDarkBackground ? SystemColors.ControlLight : SystemColors.ControlDarkDark;

        DesignerUtils.DrawFrame(_graphics, region, style, backColor);

        Color pixelColor = _bitmap.GetPixel(_bounds.Width / 2, _bounds.Height / 2);
        pixelColor.ToArgb().Should().Be(expectedColor.ToArgb());
    }

    [WinFormsTheory]
    [BoolData]
    public void DrawNoResizeHandle_ShouldNotThrow_WhenCalledWithValidParameters(bool isPrimary)
    {
        Exception? exception = Record.Exception(() =>
            DesignerUtils.DrawNoResizeHandle(_graphics, _bounds, isPrimary));

        exception.Should().BeNull();
    }

    [WinFormsTheory]
    [BoolData]
    public void DrawNoResizeHandle_ShouldDrawHandle_BasedOnSelectionType(bool isPrimary)
    {
        DesignerUtils.DrawNoResizeHandle(_graphics, _bounds, isPrimary);

        Color pixelColor = _bitmap.GetPixel(_bounds.Left + 1, _bounds.Top + 1);
        pixelColor.ToArgb().Should().NotBe(Color.Empty.ToArgb());
    }

    [WinFormsFact]
    public void GenerateSnapShot_ShouldGenerateImage_WhenControlSupportsWM_PRINT()
    {
        int borderSize = 2;
        double opacity = 0.5;
        Color backColor = Color.White;

        DesignerUtils.GenerateSnapShot(Button, out Bitmap image, borderSize, opacity, backColor);

        using (image)
        {
            image.Should().BeOfType<Bitmap>();
            image.Width.Should().BeGreaterThan(0);
            image.Height.Should().BeGreaterThan(0);
        }
    }

    [WinFormsFact]
    public void GenerateSnapShot_ShouldApplyOpacity()
    {
        int borderSize = 0;
        double opacity = 0.5;
        Color backColor = Color.White;

        DesignerUtils.GenerateSnapShot(Button, out Bitmap image, borderSize, opacity, backColor);

        using (image)
        {
            image.Should().BeOfType<Bitmap>();
            Color pixelColor = image.GetPixel(image.Width / 2, image.Height / 2);
            pixelColor.A.Should().BeLessThan(255);
        }
    }

    [WinFormsFact]
    public void GenerateSnapShot_ShouldDrawBorder_WhenBorderSizeIsGreaterThanZero()
    {
        int borderSize = 2;
        double opacity = 1.0;
        Color backColor = Color.White;

        DesignerUtils.GenerateSnapShot(Button, out Bitmap image, borderSize, opacity, backColor);

        using (image)
        {
            image.Should().BeOfType<Bitmap>();
            Color borderPixelColor = image.GetPixel(0, 0);
            borderPixelColor.Should().NotBe(backColor);
        }
    }

    [WinFormsTheory]
    [InlineData(AdornmentType.GrabHandle, 7, 7)]
    [InlineData(AdornmentType.ContainerSelector, 15, 15)]
    [InlineData((AdornmentType)999, 0, 0)]
    internal void GetAdornmentDimensions_ShouldReturnExpectedSize(AdornmentType adornmentType, int expectedWidth, int expectedHeight)
    {
        Size result = DesignerUtils.GetAdornmentDimensions(adornmentType);

        result.Width.Should().Be(expectedWidth);
        result.Height.Should().Be(expectedHeight);
    }

    [WinFormsFact]
    public void UseSnapLines_ShouldReturnTrue_WhenProviderHasNoSnapLinesProperty()
    {
        Mock<IServiceProvider> mockProvider = new();
        mockProvider.Setup(p => p.GetService(typeof(DesignerOptionService))).Returns(null!);

        DesignerUtils.UseSnapLines(mockProvider.Object).Should().BeTrue();
    }

    [WinFormsFact]
    public void GetOptionValue_ShouldReturnNull_WhenProviderIsNull()
    {
        object? result = DesignerUtils.GetOptionValue(provider: null, name: "SomeOption");

        result.Should().BeNull();
    }

    [WinFormsFact]
    public void GetOptionValue_ShouldReturnNull_WhenDesignerOptionServiceIsNotAvailable()
    {
        Mock<IServiceProvider> mockProvider = new();
        mockProvider.Setup(p => p.GetService(typeof(DesignerOptionService))).Returns(null!);

        object? result = DesignerUtils.GetOptionValue(mockProvider.Object, "SomeOption");

        result.Should().BeNull();
    }

    [WinFormsFact]
    public void GetOptionValue_ShouldReturnNull_WhenIDesignerOptionServiceIsNotAvailable()
    {
        Mock<IServiceProvider> mockProvider = new();
        mockProvider.Setup(p => p.GetService(typeof(IDesignerOptionService))).Returns(null!);

        object? result = DesignerUtils.GetOptionValue(mockProvider.Object, "SomeOption");

        result.Should().BeNull();
    }

    [WinFormsFact]
    public void GetOptionValue_ShouldReturnValue_WhenIDesignerOptionServiceHasOption()
    {
        Mock<IServiceProvider> mockProvider = new();
        Mock<IDesignerOptionService> mockOptionService = new();
        mockOptionService.Setup(o => o.GetOptionValue("WindowsFormsDesigner\\General", "SomeOption")).Returns("ExpectedValue");
        mockProvider.Setup(p => p.GetService(typeof(IDesignerOptionService))).Returns(mockOptionService.Object);

        object? result = DesignerUtils.GetOptionValue(mockProvider.Object, "SomeOption");

        result.Should().Be("ExpectedValue");
    }

    [WinFormsFact]
    public void GenerateSnapShotWithBitBlt_ShouldGenerateImage_WhenControlIsValid()
    {
        DesignerUtils.GenerateSnapShotWithBitBlt(Button, out Bitmap image);

        using (image)
        {
            image.Should().BeOfType<Bitmap>();
            image.Width.Should().BeGreaterThan(0);
            image.Height.Should().BeGreaterThan(0);
        }
    }

    [WinFormsFact]
    public void GenerateSnapShotWithBitBlt_ShouldNotThrow_WhenControlIsValid()
    {
        Exception? exception = Record.Exception(() =>
        {
            DesignerUtils.GenerateSnapShotWithBitBlt(Button, out Bitmap image);
            using (image)
            {
                image.Should().BeOfType<Bitmap>();
            }
        });

        exception.Should().BeNull();
    }

    [WinFormsFact]
    public void GenerateSnapShotWithWM_PRINT_ShouldReturnTrue_WhenControlRespondsToWM_PRINT()
    {
        bool result = DesignerUtils.GenerateSnapShotWithWM_PRINT(Button, out Bitmap image);

        result.Should().BeTrue();

        using (image)
        {
            image.Should().BeOfType<Bitmap>();
            image.Width.Should().BeGreaterThan(0);
            image.Height.Should().BeGreaterThan(0);
        }
    }

    [WinFormsTheory]
    [InlineData(SelectionBorderGlyphType.Top, 30, 20, 2, 28, 18, 24, 2)]
    [InlineData(SelectionBorderGlyphType.Bottom, 10, 20, 2, 8, 40, 24, 2)]
    [InlineData(SelectionBorderGlyphType.Left, 10, 20, 2, 8, 18, 2, 24)]
    [InlineData(SelectionBorderGlyphType.Right, 10, 20, 2, 30, 18, 2, 24)]
    [InlineData(SelectionBorderGlyphType.Body, 10, 20, 2, 10, 20, 20, 20)]
    [InlineData((SelectionBorderGlyphType)999, 10, 20, 2, 0, 0, 0, 0)]
    internal void GetBoundsForSelectionType_ShouldReturnExpectedBounds(
        SelectionBorderGlyphType type,
        int x,
        int y,
        int borderSize,
        int expectedX,
        int expectedY,
        int expectedWidth,
        int expectedHeight)
    {
        Rectangle originalBounds = new(x, y, 20, 20);

        Rectangle result = DesignerUtils.GetBoundsForSelectionType(originalBounds, type, borderSize);

        result.X.Should().Be(expectedX);
        result.Y.Should().Be(expectedY);
        result.Width.Should().Be(expectedWidth);
        result.Height.Should().Be(expectedHeight);
    }

    [WinFormsFact]
    public void ApplyListViewThemeStyles_ShouldSetWindowThemeToExplorer()
    {
        using ListView listView = new();
        Exception? exception = Record.Exception(() => DesignerUtils.ApplyListViewThemeStyles(listView));
        exception.Should().BeNull();
        listView.IsHandleCreated.Should().BeTrue();
    }

    [WinFormsFact]
    public void ApplyListViewThemeStyles_ShouldThrowArgumentNullException_WhenListViewIsNull()
    {
        Action action = () => DesignerUtils.ApplyListViewThemeStyles(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [WinFormsFact]
    public void ApplyTreeViewThemeStyles_ShouldSetWindowThemeToExplorer()
    {
        using TreeView treeView = new();
        Exception? exception = Record.Exception(() => DesignerUtils.ApplyTreeViewThemeStyles(treeView));
        exception.Should().BeNull();
        treeView.IsHandleCreated.Should().BeTrue();
    }

    [WinFormsFact]
    public void ApplyTreeViewThemeStyles_ShouldThrowArgumentNullException_WhenTreeViewIsNull()
    {
        Action action = () => DesignerUtils.ApplyTreeViewThemeStyles(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [WinFormsFact]
    public void CheckForNestedContainer_ShouldReturnSameContainer_WhenNotNested()
    {
        Mock<IContainer> nestedContainer = new();
        IContainer? result = DesignerUtils.CheckForNestedContainer(nestedContainer.Object);
        result.Should().BeSameAs(nestedContainer.Object);
    }

    [WinFormsFact]
    public void FilterGenericTypes_ShouldReturnNull_WhenTypesIsNull()
    {
        ICollection? result = DesignerUtils.FilterGenericTypes(null);
        result.Should().BeNull();
    }

    [WinFormsFact]
    public void FilterGenericTypes_ShouldReturnEmptyCollection_WhenTypesIsEmpty()
    {
        ICollection emptyCollection = Array.Empty<Type>();
        ICollection? result = DesignerUtils.FilterGenericTypes(emptyCollection);
        result.Should().BeSameAs(emptyCollection);
    }

    [WinFormsFact]
    public void FilterGenericTypes_ShouldFilterOutGenericTypes()
    {
        ICollection types = new List<Type>
        {
            typeof(List<>),
            typeof(int),
            typeof(Dictionary<,>),
            typeof(string)
        };

        ICollection? result = DesignerUtils.FilterGenericTypes(types);

        result.Should().NotBeNull();
        result.Should().BeOfType<List<Type>>();
        result.Should().BeEquivalentTo(new List<Type> { typeof(int), typeof(string) });
    }

    [WinFormsFact]
    public void GetUniqueSiteName_ShouldReturnNull_WhenNameIsNullOrEmpty()
    {
        Mock<IDesignerHost> mockHost = new();
        string? result = DesignerUtils.GetUniqueSiteName(mockHost.Object, null);
        result.Should().BeNull();

        result = DesignerUtils.GetUniqueSiteName(mockHost.Object, string.Empty);
        result.Should().BeNull();
    }

    [WinFormsFact]
    public void GetUniqueSiteName_ShouldReturnName_WhenNameIsNotInUseAndValid()
    {
        Mock<IDesignerHost> mockHost = new();
        Mock<INameCreationService> mockNameCreationService = new();
        mockNameCreationService.Setup(s => s.IsValidName("TestName")).Returns(true);

        mockHost.Setup(h => h.GetService(typeof(INameCreationService))).Returns(mockNameCreationService.Object);

        ComponentCollection components = new(Array.Empty<IComponent>());
        Mock<IContainer> mockContainer = new();
        mockContainer.Setup(c => c.Components).Returns(components);
        mockHost.Setup(h => h.Container).Returns(mockContainer.Object);

        string? result = DesignerUtils.GetUniqueSiteName(mockHost.Object, "TestName");

        result.Should().Be("TestName");
    }

    [WinFormsTheory]
    [InlineData(0, 0, 100, 100, 10, 10, ToolboxSnapDragDropEventArgs.SnapDirection.Top, false, 0, 10)]
    [InlineData(0, 0, 100, 100, 10, 10, ToolboxSnapDragDropEventArgs.SnapDirection.Bottom, false, 0, -90)]
    [InlineData(0, 0, 100, 100, 10, 10, ToolboxSnapDragDropEventArgs.SnapDirection.Left, false, 10, 0)]
    [InlineData(0, 0, 100, 100, 10, 10, ToolboxSnapDragDropEventArgs.SnapDirection.Right, false, -90, 0)]
    [InlineData(0, 0, 100, 100, 10, 10, ToolboxSnapDragDropEventArgs.SnapDirection.Top, true, 0, 10)]
    [InlineData(0, 0, 100, 100, 10, 10, ToolboxSnapDragDropEventArgs.SnapDirection.Right, true, -10, 0)]
    internal void GetBoundsFromToolboxSnapDragDropInfo_ShouldReturnExpectedBounds(
        int x,
        int y,
        int width,
        int height,
        int offsetX,
        int offsetY,
        ToolboxSnapDragDropEventArgs.SnapDirection snapDirection,
        bool isMirrored,
        int expectedX,
        int expectedY)
    {
        Rectangle originalBounds = new(x, y, width, height);
        ToolboxSnapDragDropEventArgs args = new(
         snapDirection,
         new Point(offsetX, offsetY),
         new DragEventArgs(
             null,
             0,
             0,
             0,
             DragDropEffects.None,
             DragDropEffects.None));

        Rectangle result = DesignerUtils.GetBoundsFromToolboxSnapDragDropInfo(
            e: args,
            originalBounds: originalBounds,
            isMirrored: isMirrored);

        result.X.Should().Be(expectedX);
        result.Y.Should().Be(expectedY);
    }

    [WinFormsTheory]
    [InlineData(100, 50, "Arial", 10, ContentAlignment.TopLeft, 14)]
    [InlineData(100, 50, "Arial", 10, ContentAlignment.TopCenter, 14)]
    [InlineData(100, 50, "Arial", 10, ContentAlignment.TopRight, 14)]
    [InlineData(100, 50, "Arial", 10, ContentAlignment.MiddleLeft, 31)]
    [InlineData(100, 50, "Arial", 10, ContentAlignment.MiddleCenter, 31)]
    [InlineData(100, 50, "Arial", 10, ContentAlignment.MiddleRight, 31)]
    [InlineData(100, 50, "Arial", 10, ContentAlignment.BottomLeft, 48)]
    [InlineData(100, 50, "Arial", 10, ContentAlignment.BottomCenter, 48)]
    [InlineData(100, 50, "Arial", 10, ContentAlignment.BottomRight, 48)]
    [InlineData(200, 100, "Times New Roman", 12, ContentAlignment.TopLeft, 16)]
    [InlineData(200, 100, "Times New Roman", 12, ContentAlignment.MiddleCenter, 57)]
    [InlineData(200, 100, "Times New Roman", 12, ContentAlignment.BottomRight, 97)]
    public void GetTextBaseline_ShouldReturnExpectedBaseline_WithVariousFontsAndBounds(
        int width, int height, string fontFamily, float fontSize, ContentAlignment alignment, int expectedBaseline)
    {
        using Button button = new()
        {
            Width = width,
            Height = height,
            Font = new Font(fontFamily, fontSize)
        };

        int baseline = DesignerUtils.GetTextBaseline(button, alignment);

        baseline.Should().Be(expectedBaseline);
    }

    [WinFormsFact]
    public void GetTextBaseline_ShouldThrowNullException_WhenControlIsNull()
    {
        Action action = () => DesignerUtils.GetTextBaseline(null!, ContentAlignment.TopLeft);

        action.Should().Throw<NullReferenceException>();
    }
}
