﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// (C) 2004 Ximian, Inc. http://www.ximian.com
// Copyright (C) 2004,2006-2007 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace System.Drawing.Tests;

public class BitmapTests : FileCleanupTestBase
{
    public static IEnumerable<object[]> Ctor_FilePath_TestData()
    {
        yield return new object[] { "16x16_one_entry_4bit.ico", 16, 16, PixelFormat.Format32bppArgb, ImageFormat.Icon };
        yield return new object[] { "bitmap_173x183_indexed_8bit.bmp", 173, 183, PixelFormat.Format8bppIndexed, ImageFormat.Bmp };
        yield return new object[] { "16x16_nonindexed_24bit.png", 16, 16, PixelFormat.Format24bppRgb, ImageFormat.Png };
    }

    [Theory]
    [MemberData(nameof(Ctor_FilePath_TestData))]
    public void Ctor_FilePath(string filename, int width, int height, PixelFormat pixelFormat, ImageFormat rawFormat)
    {
        using Bitmap bitmap = new(Helpers.GetTestBitmapPath(filename));
        Assert.Equal(width, bitmap.Width);
        Assert.Equal(height, bitmap.Height);
        Assert.Equal(pixelFormat, bitmap.PixelFormat);
        Assert.Equal(rawFormat, bitmap.RawFormat);
    }

    [Theory]
    [MemberData(nameof(Ctor_FilePath_TestData))]
    public void Ctor_FilePath_UseIcm(string filename, int width, int height, PixelFormat pixelFormat, ImageFormat rawFormat)
    {
        foreach (bool useIcm in new bool[] { true, false })
        {
            using Bitmap bitmap = new(Helpers.GetTestBitmapPath(filename), useIcm);
            Assert.Equal(width, bitmap.Width);
            Assert.Equal(height, bitmap.Height);
            Assert.Equal(pixelFormat, bitmap.PixelFormat);
            Assert.Equal(rawFormat, bitmap.RawFormat);
        }
    }

    [Fact]
    public void Ctor_NullFilePath_ThrowsArgumentNullException()
    {
        AssertExtensions.Throws<ArgumentNullException>("path", () => new Bitmap((string)null));
        AssertExtensions.Throws<ArgumentNullException>("path", () => new Bitmap((string)null, false));
    }

    [Theory]
    [InlineData("", "path")]
    [InlineData("\0", "path")]
    [InlineData("NoSuchPath", null)]
    public void Ctor_InvalidFilePath_ThrowsArgumentException(string filename, string? paramName)
    {
        AssertExtensions.Throws<ArgumentException>(paramName, null, () => new Bitmap(filename));
        AssertExtensions.Throws<ArgumentException>(paramName, null, () => new Bitmap(filename, false));
        AssertExtensions.Throws<ArgumentException>(paramName, null, () => new Bitmap(filename, true));
    }

    [Fact]
    public void Ctor_Type_ResourceName()
    {
        using Bitmap bitmap = new(typeof(BitmapTests), "bitmap_173x183_indexed_8bit.bmp");
        Assert.Equal(173, bitmap.Width);
        Assert.Equal(183, bitmap.Height);
        Assert.Equal(PixelFormat.Format8bppIndexed, bitmap.PixelFormat);
        Assert.Equal(ImageFormat.Bmp, bitmap.RawFormat);
    }

    [Fact]
    public void Ctor_NullType_ThrowsArgumentNullException()
    {
        AssertExtensions.Throws<ArgumentNullException, NullReferenceException>("type", () => new Bitmap(null, "name"));
    }

    [Theory]
    [InlineData(typeof(Bitmap), "")]
    [InlineData(typeof(Bitmap), "bitmap_173x183_indexed_8bit.bmp")]
    [InlineData(typeof(BitmapTests), "bitmap_173x183_INDEXED_8bit.bmp")]
    [InlineData(typeof(BitmapTests), "empty.file")]
    public void Ctor_InvalidResource_ThrowsArgumentException(Type type, string resource)
    {
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(type, resource));
    }

    [Fact]
    public void Ctor_InvalidResource_ThrowsArgumentNullException()
    {
        AssertExtensions.Throws<ArgumentNullException, ArgumentException>("resource", null, () => new Bitmap(typeof(Bitmap), null));
    }

    [Theory]
    [MemberData(nameof(Ctor_FilePath_TestData))]
    public void Ctor_Stream(string filename, int width, int height, PixelFormat pixelFormat, ImageFormat rawFormat)
    {
        using Stream stream = File.OpenRead(Helpers.GetTestBitmapPath(filename));
        using Bitmap bitmap = new(stream);
        Assert.Equal(width, bitmap.Width);
        Assert.Equal(height, bitmap.Height);
        Assert.Equal(pixelFormat, bitmap.PixelFormat);
        Assert.Equal(rawFormat, bitmap.RawFormat);
    }

    [Theory]
    [MemberData(nameof(Ctor_FilePath_TestData))]
    public void Ctor_Stream_UseIcm(string filename, int width, int height, PixelFormat pixelFormat, ImageFormat rawFormat)
    {
        foreach (bool useIcm in new bool[] { true, false })
        {
            using Stream stream = File.OpenRead(Helpers.GetTestBitmapPath(filename));
            using Bitmap bitmap = new(stream, useIcm);
            Assert.Equal(width, bitmap.Width);
            Assert.Equal(height, bitmap.Height);
            Assert.Equal(pixelFormat, bitmap.PixelFormat);
            Assert.Equal(rawFormat, bitmap.RawFormat);
        }
    }

    [Fact]
    public void Ctor_NullStream_ThrowsArgumentNullException()
    {
        AssertExtensions.Throws<ArgumentNullException, ArgumentException>("stream", null, () => new Bitmap((Stream)null));
        AssertExtensions.Throws<ArgumentNullException, ArgumentException>("stream", null, () => new Bitmap((Stream)null, false));
    }

    [Fact]
    public void Ctor_InvalidBytesInStream_ThrowsArgumentException()
    {
        using MemoryStream stream = new([]);
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(stream));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(stream, false));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(stream, true));
    }

    [Theory]
    [InlineData(10, 10)]
    [InlineData(5, 15)]
    public void Ctor_Width_Height(int width, int height)
    {
        using Bitmap bitmap = new(width, height);
        Assert.Equal(width, bitmap.Width);
        Assert.Equal(height, bitmap.Height);
        Assert.Equal(PixelFormat.Format32bppArgb, bitmap.PixelFormat);
        Assert.Equal(ImageFormat.MemoryBmp, bitmap.RawFormat);
    }

    [Theory]
    [InlineData(10, 10, PixelFormat.Format1bppIndexed)]
    [InlineData(10, 10, PixelFormat.Format8bppIndexed)]
    [InlineData(1, 1, PixelFormat.Format16bppArgb1555)]
    [InlineData(1, 1, PixelFormat.Format16bppRgb555)]
    [InlineData(1, 1, PixelFormat.Format16bppRgb565)]
    [InlineData(1, 1, PixelFormat.Format16bppGrayScale)]
    [InlineData(1, 1, PixelFormat.Format24bppRgb)]
    [InlineData(1, 1, PixelFormat.Format32bppRgb)]
    [InlineData(5, 15, PixelFormat.Format32bppArgb)]
    [InlineData(1, 1, PixelFormat.Format32bppPArgb)]
    [InlineData(10, 10, PixelFormat.Format48bppRgb)]
    [InlineData(10, 10, PixelFormat.Format4bppIndexed)]
    [InlineData(1, 1, PixelFormat.Format64bppArgb)]
    [InlineData(1, 1, PixelFormat.Format64bppPArgb)]
    public void Ctor_Width_Height_PixelFormat(int width, int height, PixelFormat pixelFormat)
    {
        using Bitmap bitmap = new(width, height, pixelFormat);
        Assert.Equal(width, bitmap.Width);
        Assert.Equal(height, bitmap.Height);
        Assert.Equal(pixelFormat, bitmap.PixelFormat);
        Assert.Equal(ImageFormat.MemoryBmp, bitmap.RawFormat);
    }

    public static IEnumerable<object[]> Ctor_Width_Height_Stride_PixelFormat_Scan0_TestData()
    {
        yield return new object[] { 10, 10, 0, PixelFormat.Format8bppIndexed, IntPtr.Zero };
        yield return new object[] { 5, 15, int.MaxValue, PixelFormat.Format32bppArgb, IntPtr.Zero };
        yield return new object[] { 5, 15, int.MinValue, PixelFormat.Format24bppRgb, IntPtr.Zero };
        yield return new object[] { 1, 1, 1, PixelFormat.Format1bppIndexed, IntPtr.Zero };
    }

    [Theory]
    [MemberData(nameof(Ctor_Width_Height_Stride_PixelFormat_Scan0_TestData))]
    public void Ctor_Width_Height_Stride_PixelFormat_Scan0(int width, int height, int stride, PixelFormat pixelFormat, IntPtr scan0)
    {
        using Bitmap bitmap = new(width, height, stride, pixelFormat, scan0);
        Assert.Equal(width, bitmap.Width);
        Assert.Equal(height, bitmap.Height);
        Assert.Equal(pixelFormat, bitmap.PixelFormat);
        Assert.Equal(ImageFormat.MemoryBmp, bitmap.RawFormat);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(ushort.MaxValue * 513)]
    [InlineData(int.MaxValue)]
    public void Ctor_InvalidWidth_ThrowsArgumentException(int width)
    {
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(width, 1));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(width, 1, Graphics.FromImage(new Bitmap(1, 1))));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(new Bitmap(1, 1), width, 1));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(new Bitmap(1, 1), new Size(width, 1)));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(width, 1, PixelFormat.Format16bppArgb1555));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(width, 1, 0, PixelFormat.Format16bppArgb1555, IntPtr.Zero));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(ushort.MaxValue * 513)]
    [InlineData(int.MaxValue)]
    public void Ctor_InvalidHeight_ThrowsArgumentException(int height)
    {
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(1, height));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(1, height, Graphics.FromImage(new Bitmap(1, 1))));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(new Bitmap(1, 1), 1, height));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(new Bitmap(1, 1), new Size(1, height)));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(1, height, PixelFormat.Format16bppArgb1555));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(1, height, 0, PixelFormat.Format16bppArgb1555, IntPtr.Zero));
    }

    [Theory]
    [InlineData(PixelFormat.Undefined - 1)]
    [InlineData(PixelFormat.Undefined)]
    [InlineData(PixelFormat.Gdi - 1)]
    [InlineData(PixelFormat.Max)]
    [InlineData(PixelFormat.Indexed)]
    [InlineData(PixelFormat.Gdi)]
    [InlineData(PixelFormat.Alpha)]
    [InlineData(PixelFormat.PAlpha)]
    [InlineData(PixelFormat.Extended)]
    [InlineData(PixelFormat.Canonical)]
    public void Ctor_InvalidPixelFormat_ThrowsArgumentException(PixelFormat format)
    {
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(1, 1, format));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(1, 1, 0, format, IntPtr.Zero));
    }

    [Fact]
    public void Ctor_InvalidScan0_ThrowsArgumentException()
    {
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(1, 1, 0, PixelFormat.Format16bppArgb1555, 10));
    }

    public static IEnumerable<object[]> Image_TestData()
    {
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format16bppRgb555), 1, 1 };
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format16bppRgb565), 1, 1 };
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format24bppRgb), 1, 1 };
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format32bppArgb), 1, 1 };
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format32bppPArgb), 1, 1 };
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format48bppRgb), 1, 1 };
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format64bppArgb), 1, 1 };
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format64bppPArgb), 1, 1 };

        yield return new object[] { new Bitmap(Helpers.GetTestBitmapPath("16x16_one_entry_4bit.ico")), 16, 16 };
        yield return new object[] { new Bitmap(Helpers.GetTestBitmapPath("16x16_nonindexed_24bit.png")), 32, 48 };
    }

    [Theory]
    [MemberData(nameof(Image_TestData))]
    public void Ctor_Width_Height_Graphics(Bitmap image, int width, int height)
    {
        using Graphics graphics = Graphics.FromImage(image);
        using Bitmap bitmap = new(width, height, graphics);
        Assert.Equal(width, bitmap.Width);
        Assert.Equal(height, bitmap.Height);
        Assert.Equal(PixelFormat.Format32bppPArgb, bitmap.PixelFormat);
        Assert.Equal(ImageFormat.MemoryBmp, bitmap.RawFormat);
    }

    [Fact]
    public void Ctor_NullGraphics_ThrowsArgumentNullException()
    {
        AssertExtensions.Throws<ArgumentNullException>("g", null, () => new Bitmap(1, 1, null));
    }

    [Fact]
    public void Ctor_Image()
    {
        using Bitmap image = new(Helpers.GetTestBitmapPath("16x16_one_entry_4bit.ico"));
        using Bitmap bitmap = new(image);
        Assert.Equal(16, bitmap.Width);
        Assert.Equal(16, bitmap.Height);
        Assert.Equal(PixelFormat.Format32bppArgb, bitmap.PixelFormat);
        Assert.Equal(ImageFormat.MemoryBmp, bitmap.RawFormat);
    }

    [Fact]
    public void Ctor_NullImageWithoutSize_ThrowsNullReferenceException()
    {
        Assert.Throws<NullReferenceException>(() => new Bitmap((Image)null));
    }

    [Theory]
    [MemberData(nameof(Image_TestData))]
    public void Ctor_Image_Width_Height(Image image, int width, int height)
    {
        using Bitmap bitmap = new(image, width, height);
        Assert.Equal(width, bitmap.Width);
        Assert.Equal(height, bitmap.Height);
        Assert.Equal(PixelFormat.Format32bppArgb, bitmap.PixelFormat);
        Assert.Equal(ImageFormat.MemoryBmp, bitmap.RawFormat);
    }

    [Theory]
    [MemberData(nameof(Image_TestData))]
    public void Ctor_Size(Image image, int width, int height)
    {
        using Bitmap bitmap = new(image, new Size(width, height));
        Assert.Equal(width, bitmap.Width);
        Assert.Equal(height, bitmap.Height);
        Assert.Equal(PixelFormat.Format32bppArgb, bitmap.PixelFormat);
        Assert.Equal(ImageFormat.MemoryBmp, bitmap.RawFormat);
    }

    [Fact]
    public void Ctor_NullImageWithSize_ThrowsArgumentNullException()
    {
        AssertExtensions.Throws<ArgumentNullException>("original", "image", () => new Bitmap(null, new Size(1, 2)));
        AssertExtensions.Throws<ArgumentNullException>("original", "image", () => new Bitmap(null, 1, 2));
    }

    [Fact]
    public void Ctor_DisposedImage_ThrowsArgumentException()
    {
        Bitmap image = new(1, 1);
        image.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(image));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(image, 1, 1));
        AssertExtensions.Throws<ArgumentException>(null, () => new Bitmap(image, new Size(1, 1)));
    }

    public static IEnumerable<object[]> Clone_TestData()
    {
        yield return new object[] { new Bitmap(3, 3, PixelFormat.Format32bppArgb), new Rectangle(0, 0, 3, 3), PixelFormat.Format32bppArgb };
        yield return new object[] { new Bitmap(3, 3, PixelFormat.Format32bppArgb), new Rectangle(0, 0, 3, 3), PixelFormat.Format24bppRgb };
        yield return new object[] { new Bitmap(3, 3, PixelFormat.Format1bppIndexed), new Rectangle(1, 1, 1, 1), PixelFormat.Format64bppArgb };
        yield return new object[] { new Bitmap(3, 3, PixelFormat.Format64bppPArgb), new Rectangle(1, 1, 1, 1), PixelFormat.Format16bppRgb565 };
    }

    [Theory]
    [MemberData(nameof(Clone_TestData))]
    public void Clone_Rectangle_ReturnsExpected(Bitmap bitmap, Rectangle rectangle, PixelFormat targetFormat)
    {
        try
        {
            using Bitmap clone = bitmap.Clone(rectangle, targetFormat);
            Assert.NotSame(bitmap, clone);

            Assert.Equal(rectangle.Width, clone.Width);
            Assert.Equal(rectangle.Height, clone.Height);
            Assert.Equal(targetFormat, clone.PixelFormat);
            Assert.Equal(bitmap.RawFormat, clone.RawFormat);

            for (int x = 0; x < rectangle.Width; x++)
            {
                for (int y = 0; y < rectangle.Height; y++)
                {
                    Color expectedColor = bitmap.GetPixel(rectangle.X + x, rectangle.Y + y);
                    if (Image.IsAlphaPixelFormat(targetFormat))
                    {
                        Assert.Equal(expectedColor, clone.GetPixel(x, y));
                    }
                    else
                    {
                        Assert.Equal(Color.FromArgb(255, expectedColor.R, expectedColor.G, expectedColor.B), clone.GetPixel(x, y));
                    }
                }
            }
        }
        finally
        {
            bitmap.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(Clone_TestData))]
    public void Clone_RectangleF_ReturnsExpected(Bitmap bitmap, Rectangle rectangle, PixelFormat format)
    {
        try
        {
            using Bitmap clone = bitmap.Clone((RectangleF)rectangle, format);
            Assert.NotSame(bitmap, clone);

            Assert.Equal(rectangle.Width, clone.Width);
            Assert.Equal(rectangle.Height, clone.Height);
            Assert.Equal(format, clone.PixelFormat);
        }
        finally
        {
            bitmap.Dispose();
        }
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void Clone_ZeroWidthOrHeightRect_ThrowsArgumentException(int width, int height)
    {
        using Bitmap bitmap = new(3, 3);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.Clone(new Rectangle(0, 0, width, height), bitmap.PixelFormat));
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.Clone(new RectangleF(0, 0, width, height), bitmap.PixelFormat));
    }

    [Theory]
    [InlineData(0, 0, 4, 1)]
    [InlineData(0, 0, 1, 4)]
    [InlineData(1, 0, 3, 1)]
    [InlineData(0, 1, 1, 3)]
    [InlineData(4, 1, 1, 1)]
    [InlineData(1, 4, 1, 1)]
    public void Clone_InvalidRect_ThrowsExternalException(int x, int y, int width, int height)
    {
        using Bitmap bitmap = new(3, 3);
        Assert.Throws<ExternalException>(() => bitmap.Clone(new Rectangle(x, y, width, height), bitmap.PixelFormat));
        Assert.Throws<ExternalException>(() => bitmap.Clone(new RectangleF(x, y, width, height), bitmap.PixelFormat));
    }

    [Theory]
    [InlineData(PixelFormat.Max)]
    [InlineData(PixelFormat.Indexed)]
    [InlineData(PixelFormat.Gdi)]
    [InlineData(PixelFormat.Alpha)]
    [InlineData(PixelFormat.PAlpha)]
    [InlineData(PixelFormat.Extended)]
    [InlineData(PixelFormat.Format16bppGrayScale)]
    [InlineData(PixelFormat.Canonical)]
    public void Clone_InvalidPixelFormat_ThrowsExternalException(PixelFormat format)
    {
        using Bitmap bitmap = new(1, 1);
        Assert.Throws<ExternalException>(() => bitmap.Clone(new Rectangle(0, 0, 1, 1), format));
        Assert.Throws<ExternalException>(() => bitmap.Clone(new RectangleF(0, 0, 1, 1), format));
    }

    [Fact]
    public void Clone_GrayscaleFormat_ThrowsExternalException()
    {
        using Bitmap bitmap = new(1, 1, PixelFormat.Format16bppGrayScale);
        Assert.Throws<ExternalException>(() => bitmap.Clone(new Rectangle(0, 0, 1, 1), PixelFormat.Format32bppArgb));
        Assert.Throws<ExternalException>(() => bitmap.Clone(new RectangleF(0, 0, 1, 1), PixelFormat.Format32bppArgb));
    }

    [Fact]
    public void Clone_ValidBitmap_Success()
    {
        using Bitmap bitmap = new(1, 1);
        using Bitmap clone = Assert.IsType<Bitmap>(bitmap.Clone());
        Assert.NotSame(bitmap, clone);
        Assert.Equal(1, clone.Width);
        Assert.Equal(1, clone.Height);
    }

    [Fact]
    public void Clone_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, bitmap.Clone);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.Clone(new Rectangle(0, 0, 1, 1), PixelFormat.Format32bppArgb));
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.Clone(new RectangleF(0, 0, 1, 1), PixelFormat.Format32bppArgb));
    }

    [Fact]
    public void GetFrameCount_NewBitmap_ReturnsZero()
    {
        using Bitmap bitmap = new(1, 1);
        Assert.Equal(1, bitmap.GetFrameCount(FrameDimension.Page));
        Assert.Equal(1, bitmap.GetFrameCount(FrameDimension.Resolution));
        Assert.Equal(1, bitmap.GetFrameCount(FrameDimension.Time));
        Assert.Equal(1, bitmap.GetFrameCount(new FrameDimension(Guid.Empty)));
    }

    [Fact]
    public void GetFrameCount_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.GetFrameCount(FrameDimension.Page));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public void SelectActiveFrame_InvalidFrameIndex_ThrowsArgumentException(int index)
    {
        using Bitmap bitmap = new(1, 1);
        Assert.Equal(0, bitmap.SelectActiveFrame(FrameDimension.Page, index));
        Assert.Equal(0, bitmap.SelectActiveFrame(FrameDimension.Resolution, index));
        Assert.Equal(0, bitmap.SelectActiveFrame(FrameDimension.Time, index));
        Assert.Equal(0, bitmap.SelectActiveFrame(new FrameDimension(Guid.Empty), index));
    }

    [Fact]
    public void SelectActiveFrame_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.SelectActiveFrame(FrameDimension.Page, 0));
    }

    public static IEnumerable<object[]> GetPixel_TestData()
    {
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format1bppIndexed), 0, 0, Color.FromArgb(0, 0, 0) };
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format4bppIndexed), 0, 0, Color.FromArgb(0, 0, 0) };
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format8bppIndexed), 0, 0, Color.FromArgb(0, 0, 0) };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), 0, 0, Color.FromArgb(0, 0, 0) };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), 99, 99, Color.FromArgb(0, 0, 0) };
    }

    [Theory]
    [MemberData(nameof(GetPixel_TestData))]
    public void GetPixel_ValidPixelFormat_Success(Bitmap bitmap, int x, int y, Color color)
    {
        try
        {
            Assert.Equal(color, bitmap.GetPixel(x, y));
        }
        finally
        {
            bitmap.Dispose();
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void GetPixel_InvalidX_ThrowsArgumentOutOfRangeException(int x)
    {
        using Bitmap bitmap = new(1, 1);
        AssertExtensions.Throws<ArgumentOutOfRangeException>("x", () => bitmap.GetPixel(x, 0));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void GetPixel_InvalidY_ThrowsArgumentOutOfRangeException(int y)
    {
        using Bitmap bitmap = new(1, 1);
        AssertExtensions.Throws<ArgumentOutOfRangeException>("y", () => bitmap.GetPixel(0, y));
    }

    [Fact]
    public void GetPixel_GrayScalePixelFormat_ThrowsArgumentException()
    {
        using Bitmap bitmap = new(1, 1, PixelFormat.Format16bppGrayScale);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.GetPixel(0, 0));
    }

    [Fact]
    public void GetPixel_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.GetPixel(0, 0));
    }

    public static IEnumerable<object[]> GetHbitmap_TestData()
    {
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format32bppRgb), 1, 1 };
        yield return new object[] { new Bitmap(32, 32, PixelFormat.Format32bppArgb), 32, 32 };
        yield return new object[] { new Bitmap(512, 512, PixelFormat.Format16bppRgb555), 512, 512 };
    }

    [Theory]
    [MemberData(nameof(GetHbitmap_TestData))]
    public void GetHbitmap_FromHbitmap_ReturnsExpected(Bitmap bitmap, int width, int height)
    {
        IntPtr handle = bitmap.GetHbitmap();
        try
        {
            Assert.NotEqual(IntPtr.Zero, handle);

            using Bitmap result = Image.FromHbitmap(handle);
            Assert.Equal(width, result.Width);
            Assert.Equal(height, result.Height);
            Assert.Equal(PixelFormat.Format32bppRgb, result.PixelFormat);
            Assert.Equal(ImageFormat.MemoryBmp, result.RawFormat);
        }
        finally
        {
            bitmap.Dispose();
        }

        // Hbitmap survives original bitmap disposal.
        using (Bitmap result = Image.FromHbitmap(handle))
        {
            Assert.Equal(width, result.Width);
            Assert.Equal(height, result.Height);
            Assert.Equal(PixelFormat.Format32bppRgb, result.PixelFormat);
            Assert.Equal(ImageFormat.MemoryBmp, result.RawFormat);
        }

        // Hbitmap can be used multiple times.
        using (Bitmap result = Image.FromHbitmap(handle))
        {
            Assert.Equal(width, result.Width);
            Assert.Equal(height, result.Height);
            Assert.Equal(PixelFormat.Format32bppRgb, result.PixelFormat);
            Assert.Equal(ImageFormat.MemoryBmp, result.RawFormat);
        }
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(short.MaxValue, 1)]
    [InlineData(1, short.MaxValue)]
    public void GetHbitmap_Grayscale_ThrowsArgumentException(int width, int height)
    {
        using Bitmap bitmap = new(width, height, PixelFormat.Format16bppGrayScale);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.GetHbitmap());
    }

    [Fact]
    public void GetHbitmap_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.GetHbitmap());
    }

    [Fact]
    public void FromHbitmap_InvalidHandle_ThrowsExternalException()
    {
        Assert.Throws<ExternalException>(() => Image.FromHbitmap(IntPtr.Zero));
        Assert.Throws<ExternalException>(() => Image.FromHbitmap(10));
    }

    public static IEnumerable<object[]> FromHicon_Icon_TestData()
    {
        yield return new object[] { new Icon(Helpers.GetTestBitmapPath("16x16_one_entry_4bit.ico")), 16, 16 };
        yield return new object[] { new Icon(Helpers.GetTestBitmapPath("32x32_one_entry_4bit.ico")), 32, 32 };
        yield return new object[] { new Icon(Helpers.GetTestBitmapPath("64x64_one_entry_8bit.ico")), 64, 64 };
        yield return new object[] { new Icon(Helpers.GetTestBitmapPath("96x96_one_entry_8bit.ico")), 96, 96 };
    }

    [Theory]
    [MemberData(nameof(FromHicon_Icon_TestData))]
    public void FromHicon_IconHandle_ReturnsExpected(Icon icon, int width, int height)
    {
        IntPtr handle;
        try
        {
            GetHicon_FromHicon_ReturnsExpected(icon.Handle, width, height);
            using var bitmap = Bitmap.FromHicon(icon.Handle);
            handle = bitmap.GetHicon();
        }
        finally
        {
            icon.Dispose();
        }

        // Hicon survives bitmap and icon disposal.
        GetHicon_FromHicon_ReturnsExpected(handle, width, height);
    }

    public static IEnumerable<object[]> FromHicon_TestData()
    {
        yield return new object[] { new Bitmap(1, 1, PixelFormat.Format32bppRgb).GetHicon(), 1, 1 };
        yield return new object[] { new Bitmap(32, 32, PixelFormat.Format32bppRgb).GetHicon(), 32, 32 };
        yield return new object[] { new Bitmap(512, 512, PixelFormat.Format16bppRgb555).GetHicon(), 512, 512 };
    }

    [Theory]
    [MemberData(nameof(FromHicon_TestData))]
    public void GetHicon_FromHicon_ReturnsExpected(IntPtr handle, int width, int height)
    {
        Assert.NotEqual(IntPtr.Zero, handle);

        using Bitmap result = Bitmap.FromHicon(handle);
        Assert.Equal(width, result.Width);
        Assert.Equal(height, result.Height);
        Assert.Equal(PixelFormat.Format32bppArgb, result.PixelFormat);
        Assert.Equal(ImageFormat.MemoryBmp, result.RawFormat);
        Assert.Equal(335888, result.Flags);
        Assert.Empty(result.Palette.Entries);
    }

    [Fact]
    public void GetHicon_Grayscale_ThrowsArgumentException()
    {
        using Bitmap bitmap = new(1, 1, PixelFormat.Format16bppGrayScale);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.GetHicon());
    }

    [Fact]
    public void GetHicon_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.GetHicon());
    }

    [Fact]
    public void SaveWmfAsPngDoesNotChangeImageBoundaries()
    {
        if (PlatformDetection.IsWindows7)
        {
            Assert.Skip("GDI+ 1.1 is not supported");
        }

        if (PlatformDetection.IsArmOrArm64Process)
        {
            // https://github.com/dotnet/winforms/issues/8817
            Assert.Skip("Arm precision");
        }

        string output = $"{GetTestFilePath()}.png";
        using Stream wmfStream = File.OpenRead(Helpers.GetTestBitmapPath("gdiwmfboundariesbug.wmf"));
        using Image bitmapFromWmf = Image.FromStream(wmfStream);
        bitmapFromWmf.Save(output, ImageFormat.Png);

        using Stream expectedPngStream = File.OpenRead(Helpers.GetTestBitmapPath("gdiwmfboundariesbug-output.png"));
        using Image expectedPngBitmap = Image.FromStream(expectedPngStream);

        using Stream outputPngStream = File.OpenRead(output);
        using Image outputPngBitmap = Image.FromStream(outputPngStream);

        Assert.Equal(expectedPngBitmap.HorizontalResolution, outputPngBitmap.HorizontalResolution);
        Assert.Equal(expectedPngBitmap.VerticalResolution, outputPngBitmap.VerticalResolution);
        Assert.Equal(expectedPngBitmap.Size, outputPngBitmap.Size);
        Assert.Equal(expectedPngBitmap.PhysicalDimension, outputPngBitmap.PhysicalDimension);
        Assert.Equal(expectedPngBitmap.PixelFormat, outputPngBitmap.PixelFormat);
    }

    [Fact]
    public void FromHicon_InvalidHandle_ThrowsArgumentException()
    {
        AssertExtensions.Throws<ArgumentException>(null, () => Bitmap.FromHicon(IntPtr.Zero));
        AssertExtensions.Throws<ArgumentException>(null, () => Bitmap.FromHicon(10));
    }

    [Fact]
    public void FromHicon_1bppIcon_ThrowsArgumentException()
    {
        using Icon icon = new(Helpers.GetTestBitmapPath("48x48_one_entry_1bit.ico"));
        AssertExtensions.Throws<ArgumentException>(null, () => Bitmap.FromHicon(icon.Handle));
    }

    [Fact]
    public void FromResource_InvalidHandle_ThrowsArgumentException()
    {
        AssertExtensions.Throws<ArgumentException>(null, () => Bitmap.FromResource(IntPtr.Zero, "Name"));
        AssertExtensions.Throws<ArgumentException>(null, () => Bitmap.FromResource(10, "Name"));
    }

    [Fact]
    public void FromResource_InvalidBitmapName_ThrowsArgumentException()
    {
        AssertExtensions.Throws<ArgumentException>(null, () => Bitmap.FromResource(IntPtr.Zero, "Name"));
        AssertExtensions.Throws<ArgumentException>(null, () => Bitmap.FromResource(10, "Name"));
    }

    [Fact]
    public void MakeTransparent_NoColorWithMatches_SetsMatchingPixelsToTransparent()
    {
        using Bitmap bitmap = new(10, 10);
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                if (y % 2 == 0)
                {
                    bitmap.SetPixel(x, y, Color.LightGray);
                }
                else
                {
                    bitmap.SetPixel(x, y, Color.Red);
                }
            }
        }

        bitmap.MakeTransparent();
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                if (y % 2 == 0)
                {
                    Assert.Equal(Color.FromArgb(255, 211, 211, 211), bitmap.GetPixel(x, y));
                }
                else
                {
                    Assert.Equal(Color.FromArgb(0, 0, 0, 0), bitmap.GetPixel(x, y));
                }
            }
        }
    }

    [Fact]
    public void MakeTransparent_CustomColorExists_SetsMatchingPixelsToTransparent()
    {
        using Bitmap bitmap = new(10, 10);
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                if (y % 2 == 0)
                {
                    bitmap.SetPixel(x, y, Color.Blue);
                }
                else
                {
                    bitmap.SetPixel(x, y, Color.Red);
                }
            }
        }

        bitmap.MakeTransparent(Color.Blue);
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                if (y % 2 == 0)
                {
                    Assert.Equal(Color.FromArgb(0, 0, 0, 0), bitmap.GetPixel(x, y));
                }
                else
                {
                    Assert.Equal(Color.FromArgb(255, 255, 0, 0), bitmap.GetPixel(x, y));
                }
            }
        }
    }

    [Fact]
    public void MakeTransparent_CustomColorDoesNotExist_DoesNothing()
    {
        using Bitmap bitmap = new(10, 10);
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                bitmap.SetPixel(x, y, Color.Blue);
            }
        }

        bitmap.MakeTransparent(Color.Red);
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                Assert.Equal(Color.FromArgb(255, 0, 0, 255), bitmap.GetPixel(x, y));
            }
        }
    }

    [Fact]
    public void MakeTransparent_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, bitmap.MakeTransparent);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.MakeTransparent(Color.Red));
    }

    [Fact]
    public void MakeTransparent_GrayscalePixelFormat_ThrowsArgumentException()
    {
        using Bitmap bitmap = new(1, 1, PixelFormat.Format16bppGrayScale);
        AssertExtensions.Throws<ArgumentException>(null, bitmap.MakeTransparent);

        try
        {
            bitmap.MakeTransparent(Color.Red);
        }
        catch (ExternalException)
        {
            // This stopped throwing on Windows 11 and then started again.
        }
    }

    [Fact]
    public void MakeTransparent_Icon_ThrowsInvalidOperationException()
    {
        using Bitmap bitmap = new(Helpers.GetTestBitmapPath("16x16_one_entry_4bit.ico"));
        Assert.Throws<InvalidOperationException>(() => bitmap.MakeTransparent(Color.Red));
    }

    public static IEnumerable<object[]> SetPixel_TestData()
    {
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), 0, 0, Color.FromArgb(255, 128, 128, 128) };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), 99, 99, Color.FromArgb(255, 128, 128, 128) };
    }

    [Theory]
    [MemberData(nameof(SetPixel_TestData))]
    public void SetPixel_ValidPixelFormat_Success(Bitmap bitmap, int x, int y, Color color)
    {
        bitmap.SetPixel(x, y, color);
        Assert.Equal(color, bitmap.GetPixel(x, y));
    }

    [Theory]
    [InlineData(PixelFormat.Format1bppIndexed)]
    [InlineData(PixelFormat.Format4bppIndexed)]
    [InlineData(PixelFormat.Format8bppIndexed)]
    public void SetPixel_IndexedPixelFormat_ThrowsInvalidOperationException(PixelFormat format)
    {
        using Bitmap bitmap = new(1, 1, format);
        Assert.Throws<InvalidOperationException>(() => bitmap.SetPixel(0, 0, Color.Red));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void SetPixel_InvalidX_ThrowsArgumentOutOfRangeException(int x)
    {
        using Bitmap bitmap = new(1, 1);
        AssertExtensions.Throws<ArgumentOutOfRangeException>("x", () => bitmap.SetPixel(x, 0, Color.Red));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void SetPixel_InvalidY_ThrowsArgumentOutOfRangeException(int y)
    {
        using Bitmap bitmap = new(1, 1);
        AssertExtensions.Throws<ArgumentOutOfRangeException>("y", () => bitmap.SetPixel(0, y, Color.Red));
    }

    [Fact]
    public void SetPixel_GrayScalePixelFormat_ThrowsArgumentException()
    {
        using Bitmap bitmap = new(1, 1, PixelFormat.Format16bppGrayScale);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.SetPixel(0, 0, Color.Red));
    }

    [Fact]
    public void SetPixel_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.SetPixel(0, 0, Color.Red));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(float.PositiveInfinity, float.PositiveInfinity)]
    [InlineData(float.MaxValue, float.MaxValue)]
    public void SetResolution_ValidDpi_Success(float xDpi, float yDpi)
    {
        using Bitmap bitmap = new(1, 1);
        bitmap.SetResolution(xDpi, yDpi);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(float.NaN)]
    [InlineData(float.NegativeInfinity)]
    public void SetResolution_InvalidXDpi_ThrowsArgumentException(float xDpi)
    {
        using Bitmap bitmap = new(1, 1);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.SetResolution(xDpi, 1));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(float.NaN)]
    [InlineData(float.NegativeInfinity)]
    public void SetResolution_InvalidYDpi_ThrowsArgumentException(float yDpi)
    {
        using Bitmap bitmap = new(1, 1);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.SetResolution(1, yDpi));
    }

    [Fact]
    public void SetResolution_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.SetResolution(1, 1));
    }

    public static IEnumerable<object[]> LockBits_TestData()
    {
        static Bitmap bitmap() => new(2, 2, PixelFormat.Format32bppArgb);
        yield return new object[] { bitmap(), new Rectangle(0, 0, 2, 2), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb, 8, 1 };
        yield return new object[] { bitmap(), new Rectangle(0, 0, 2, 2), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb, 8, 3 };
        yield return new object[] { bitmap(), new Rectangle(0, 0, 2, 2), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb, 8, 2 };

        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb, 400, 1 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb, 400, 3 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb, 400, 2 };

        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb, 300, 65537 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb, 300, 65539 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb, 300, 65538 };

        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format24bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb, 300, 1 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format24bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb, 300, 3 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format24bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb, 300, 2 };

        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format24bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb, 400, 65537 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format24bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb, 400, 65539 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format24bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb, 400, 65538 };

        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format8bppIndexed), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb, 300, 65537 };

        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format8bppIndexed), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed, 100, 1 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format8bppIndexed), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed, 100, 3 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format8bppIndexed), new Rectangle(0, 0, 100, 100), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed, 100, 2 };

        yield return new object[] { new Bitmap(184, 184, PixelFormat.Format1bppIndexed), new Rectangle(0, 0, 184, 184), ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed, 24, 1 };
        yield return new object[] { new Bitmap(184, 184, PixelFormat.Format1bppIndexed), new Rectangle(0, 0, 184, 184), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed, 24, 3 };
        yield return new object[] { new Bitmap(184, 184, PixelFormat.Format1bppIndexed), new Rectangle(0, 0, 184, 184), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed, 24, 2 };

        yield return new object[] { bitmap(), new Rectangle(1, 1, 1, 1), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb, 8, 1 };
        yield return new object[] { bitmap(), new Rectangle(1, 1, 1, 1), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb, 8, 3 };
        yield return new object[] { bitmap(), new Rectangle(1, 1, 1, 1), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb, 8, 2 };

        yield return new object[] { bitmap(), new Rectangle(1, 1, 1, 1), ImageLockMode.ReadOnly - 1, PixelFormat.Format32bppArgb, 8, 0 };

        yield return new object[] { bitmap(), new Rectangle(0, 0, 2, 2), ImageLockMode.WriteOnly, PixelFormat.Format16bppGrayScale, 4, 65538 };

        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed, 100, 65537 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed, 100, 65539 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format32bppRgb), new Rectangle(0, 0, 100, 100), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed, 100, 65538 };

        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format8bppIndexed), new Rectangle(0, 0, 100, 100), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb, 300, 65539 };
        yield return new object[] { new Bitmap(100, 100, PixelFormat.Format8bppIndexed), new Rectangle(0, 0, 100, 100), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb, 300, 65538 };
    }

    [Theory(Skip = "Condition not met", SkipType = typeof(PlatformDetection), SkipUnless = nameof(PlatformDetection.IsNotArm64Process))] // [ActiveIssue("https://github.com/dotnet/winforms/issues/8817")]
    [MemberData(nameof(LockBits_TestData))]
    public void LockBits_Invoke_Success(Bitmap bitmap, Rectangle rectangle, ImageLockMode lockMode, PixelFormat pixelFormat, int expectedStride, int expectedReserved)
    {
        try
        {
            BitmapData data = bitmap.LockBits(rectangle, lockMode, pixelFormat);
            Assert.Equal(pixelFormat, data.PixelFormat);
            Assert.Equal(rectangle.Width, data.Width);
            Assert.Equal(rectangle.Height, data.Height);
            Assert.Equal(expectedStride, data.Stride);

            // "Reserved" is documented as "Reserved. Do not use.", so it's not clear whether we actually need to test this in any unit tests.
            Assert.Equal(expectedReserved, data.Reserved);

            // Locking with 16bppGrayscale succeeds, but the data can't be unlocked.
            if (pixelFormat == PixelFormat.Format16bppGrayScale)
            {
                AssertExtensions.Throws<ArgumentException>(null, () => bitmap.UnlockBits(data));
            }
            else
            {
                bitmap.UnlockBits(data);
            }
        }
        finally
        {
            bitmap.Dispose();
        }
    }

    [Fact]
    public void LockBits_NullBitmapData_ThrowsArgumentNullException()
    {
        using Bitmap bitmap = new(1, 1);
        AssertExtensions.Throws<ArgumentNullException>("bitmapData", () => bitmap.LockBits(Rectangle.Empty, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb, null));
    }

    [Theory]
    [InlineData(-1, 0, 1, 1)]
    [InlineData(2, 0, 1, 1)]
    [InlineData(0, -1, 1, 1)]
    [InlineData(0, 2, 1, 1)]
    [InlineData(0, 0, -1, 1)]
    [InlineData(0, 0, 3, 1)]
    [InlineData(0, 0, 1, -1)]
    [InlineData(0, 0, 1, 3)]
    [InlineData(1, 0, 2, 1)]
    [InlineData(1, 1, 1, 0)]
    [InlineData(1, 1, 0, 1)]
    public void LockBits_InvalidRect_ThrowsArgumentException(int x, int y, int width, int height)
    {
        using Bitmap bitmap = new(2, 2);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(x, y, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat));

        BitmapData bitmapData = new();
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(x, y, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat, bitmapData));
        Assert.Equal(IntPtr.Zero, bitmapData.Scan0);
    }

    [Theory]
    [InlineData(PixelFormat.DontCare)]
    [InlineData(PixelFormat.Max)]
    [InlineData(PixelFormat.Indexed)]
    [InlineData(PixelFormat.Gdi)]
    [InlineData(PixelFormat.Alpha)]
    [InlineData(PixelFormat.PAlpha)]
    [InlineData(PixelFormat.Extended)]
    [InlineData(PixelFormat.Canonical)]
    public void LockBits_InvalidPixelFormat_ThrowsArgumentException(PixelFormat format)
    {
        using Bitmap bitmap = new(1, 1);
        foreach (ImageLockMode lockMode in Enum.GetValues(typeof(ImageLockMode)))
        {
            AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(0, 0, 1, 1), lockMode, format));

            BitmapData bitmapData = new();
            AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(0, 0, 1, 1), lockMode, format, bitmapData));
            Assert.Equal(IntPtr.Zero, bitmapData.Scan0);
        }
    }

    [Fact]
    public void LockBits_ReadOnlyGrayscale_ThrowsArgumentException()
    {
        using Bitmap bitmap = new(1, 1);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, PixelFormat.Format16bppGrayScale));
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, PixelFormat.Format16bppGrayScale, new BitmapData()));

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadWrite, PixelFormat.Format16bppGrayScale));
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadWrite, PixelFormat.Format16bppGrayScale, new BitmapData()));

        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.WriteOnly, PixelFormat.Format16bppGrayScale);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.UnlockBits(data));
    }

    [Theory]
    [InlineData((ImageLockMode)(-1))]
    [InlineData(ImageLockMode.UserInputBuffer + 1)]
    [InlineData(ImageLockMode.UserInputBuffer)]
    public void LockBits_InvalidLockMode_ThrowsArgumentException(ImageLockMode lockMode)
    {
        using Bitmap bitmap = new(1, 1);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(0, 0, 1, 1), lockMode, bitmap.PixelFormat));

        BitmapData bitmapData = new();
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(0, 0, 1, 1), lockMode, bitmap.PixelFormat, bitmapData));
        Assert.Equal(IntPtr.Zero, bitmapData.Scan0);
    }

    [Fact]
    public void LockBits_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb));

        BitmapData bitmapData = new();
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb, bitmapData));
        Assert.Equal(IntPtr.Zero, bitmapData.Scan0);
    }

    [Fact]
    public void LockBits_AlreadyLocked_ThrowsInvalidOperationException()
    {
        using Bitmap bitmap = new(1, 1);
        bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, bitmap.PixelFormat);

        Assert.Throws<InvalidOperationException>(() => bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, bitmap.PixelFormat));
        Assert.Throws<InvalidOperationException>(() => bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, bitmap.PixelFormat, new BitmapData()));

        Assert.Throws<InvalidOperationException>(() => bitmap.LockBits(new Rectangle(1, 1, 1, 1), ImageLockMode.ReadOnly, bitmap.PixelFormat));
        Assert.Throws<InvalidOperationException>(() => bitmap.LockBits(new Rectangle(1, 1, 1, 1), ImageLockMode.ReadOnly, bitmap.PixelFormat, new BitmapData()));
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(0, 2)]
    [InlineData(1, 2)]
    public void UnlockBits_InvalidHeightWidth_Nop(int offset, int invalidParameter)
    {
        using Bitmap bitmap = new(2, 2);
        BitmapData data = bitmap.LockBits(new Rectangle(offset, offset, 1, 1), ImageLockMode.ReadOnly, bitmap.PixelFormat);
        data.Height = invalidParameter;
        data.Width = invalidParameter;

        bitmap.UnlockBits(data);
    }

    [Fact]
    public void UnlockBits_Scan0Zero_Nop()
    {
        using Bitmap bitmap = new(1, 1);
        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, bitmap.PixelFormat);
        data.Scan0 = IntPtr.Zero;

        bitmap.UnlockBits(data);
    }

    [Theory]
    [InlineData(PixelFormat.Indexed)]
    [InlineData(PixelFormat.Gdi)]
    public void UnlockBits_InvalidPixelFormat_Nop(PixelFormat format)
    {
        using Bitmap bitmap = new(1, 1);
        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, bitmap.PixelFormat);
        data.PixelFormat = format;

        bitmap.UnlockBits(data);
    }

    [Fact]
    public void UnlockBits_NullBitmapData_ThrowsArgumentNullException()
    {
        using Bitmap bitmap = new(1, 1);
        AssertExtensions.Throws<ArgumentNullException>("bitmapdata", () => bitmap.UnlockBits(null));
    }

    [Fact]
    public void UnlockBits_NotLocked_ThrowsExternalException()
    {
        using Bitmap bitmap = new(1, 1);
        Assert.Throws<ExternalException>(() => bitmap.UnlockBits(new BitmapData()));
    }

    [Fact]
    public void UnlockBits_AlreadyUnlocked_ThrowsExternalException()
    {
        using Bitmap bitmap = new(1, 1);
        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, bitmap.PixelFormat);
        bitmap.UnlockBits(data);

        Assert.Throws<ExternalException>(() => bitmap.UnlockBits(new BitmapData()));
    }

    [Fact]
    public void UnlockBits_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.UnlockBits(new BitmapData()));
    }

    [Fact]
    public void Size_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.Width);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.Height);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.Size);
    }

    [Theory]
    [InlineData(PixelFormat.Format16bppArgb1555)]
    [InlineData(PixelFormat.Format16bppRgb555)]
    [InlineData(PixelFormat.Format16bppRgb565)]
    [InlineData(PixelFormat.Format32bppArgb)]
    [InlineData(PixelFormat.Format32bppPArgb)]
    [InlineData(PixelFormat.Format32bppRgb)]
    [InlineData(PixelFormat.Format24bppRgb)]
    public void CustomPixelFormat_GetPixels_ReturnsExpected(PixelFormat format)
    {
        bool alpha = Image.IsAlphaPixelFormat(format);
        int size = Image.GetPixelFormatSize(format) / 8 * 2;
        using Bitmap bitmap = new(2, 1, format);
        Color a = Color.FromArgb(128, 64, 32, 16);
        Color b = Color.FromArgb(192, 96, 48, 24);
        bitmap.SetPixel(0, 0, a);
        bitmap.SetPixel(1, 0, b);
        Color c = bitmap.GetPixel(0, 0);
        Color d = bitmap.GetPixel(1, 0);
        if (size == 4)
        {
            Assert.Equal(255, c.A);
            Assert.Equal(66, c.R);
            if (format == PixelFormat.Format16bppRgb565)
            {
                Assert.Equal(32, c.G);
            }
            else
            {
                Assert.Equal(33, c.G);
            }

            Assert.Equal(16, c.B);

            Assert.Equal(255, d.A);
            Assert.Equal(99, d.R);
            if (format == PixelFormat.Format16bppRgb565)
            {
                Assert.Equal(48, d.G);
            }
            else
            {
                Assert.Equal(49, d.G);
            }

            Assert.Equal(24, d.B);
        }
        else if (alpha)
        {
            if (format == PixelFormat.Format32bppPArgb)
            {
                Assert.Equal(a.A, c.A);
                Assert.Equal(a.R - 1, c.R);
                Assert.Equal(a.G - 1, c.G);
                Assert.Equal(a.B - 1, c.B);

                Assert.Equal(b.A, d.A);
                Assert.Equal(b.R - 1, d.R);
                Assert.Equal(b.G - 1, d.G);
                Assert.Equal(b.B - 1, d.B);
            }
            else
            {
                Assert.Equal(a, c);
                Assert.Equal(b, d);
            }
        }
        else
        {
            Assert.Equal(Color.FromArgb(255, 64, 32, 16), c);
            Assert.Equal(Color.FromArgb(255, 96, 48, 24), d);
        }

        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, 2, 1), ImageLockMode.ReadOnly, format);
        try
        {
            byte[] data = new byte[size];
            Marshal.Copy(bitmapData.Scan0, data, 0, size);
            if (format == PixelFormat.Format32bppPArgb)
            {
                Assert.Equal(Math.Ceiling((float)c.B * c.A / 255), data[0]);
                Assert.Equal(Math.Ceiling((float)c.G * c.A / 255), data[1]);
                Assert.Equal(Math.Ceiling((float)c.R * c.A / 255), data[2]);
                Assert.Equal(c.A, data[3]);
                Assert.Equal(Math.Ceiling((float)d.B * d.A / 255), data[4]);
                Assert.Equal(Math.Ceiling((float)d.G * d.A / 255), data[5]);
                Assert.Equal(Math.Ceiling((float)d.R * d.A / 255), data[6]);
                Assert.Equal(d.A, data[7]);
            }
            else if (size == 4)
            {
                switch (format)
                {
                    case PixelFormat.Format16bppRgb565:
                        Assert.Equal(2, data[0]);
                        Assert.Equal(65, data[1]);
                        Assert.Equal(131, data[2]);
                        Assert.Equal(97, data[3]);
                        break;
                    case PixelFormat.Format16bppArgb1555:
                        Assert.Equal(130, data[0]);
                        Assert.Equal(160, data[1]);
                        Assert.Equal(195, data[2]);
                        Assert.Equal(176, data[3]);
                        break;
                    case PixelFormat.Format16bppRgb555:
                        Assert.Equal(130, data[0]);
                        Assert.Equal(32, data[1]);
                        Assert.Equal(195, data[2]);
                        Assert.Equal(48, data[3]);
                        break;
                }
            }
            else
            {
                int n = 0;
                Assert.Equal(c.B, data[n++]);
                Assert.Equal(c.G, data[n++]);
                Assert.Equal(c.R, data[n++]);
                if (size % 4 == 0)
                {
                    if (format == PixelFormat.Format32bppRgb)
                    {
                        Assert.Equal(128, data[n++]);
                    }
                    else
                    {
                        Assert.Equal(c.A, data[n++]);
                    }
                }

                Assert.Equal(d.B, data[n++]);
                Assert.Equal(d.G, data[n++]);
                Assert.Equal(d.R, data[n++]);
                if (size % 4 == 0)
                {
                    if (format == PixelFormat.Format32bppRgb)
                    {
                        Assert.Equal(192, data[n++]);
                    }
                    else
                    {
                        Assert.Equal(d.A, data[n++]);
                    }
                }
            }
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }
    }

    public static TheoryData<PixelFormat, int[]> Palette_TestData => new()
    {
        { PixelFormat.Format1bppIndexed, new int[] { -16777216, -1 } },
        { PixelFormat.Format4bppIndexed, new int[] { -16777216, -8388608, -16744448, -8355840, -16777088, -8388480, -16744320, -8355712, -4144960, -65536, -16711936, -256, -16776961, -65281, -16711681, -1, } },
        { PixelFormat.Format8bppIndexed, new int[] { -16777216, -8388608, -16744448, -8355840, -16777088, -8388480, -16744320, -8355712, -4144960, -65536, -16711936, -256, -16776961, -65281, -16711681, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -16777216, -16777165, -16777114, -16777063, -16777012, -16776961, -16764160, -16764109, -16764058, -16764007, -16763956, -16763905, -16751104, -16751053, -16751002, -16750951, -16750900, -16750849, -16738048, -16737997, -16737946, -16737895, -16737844, -16737793, -16724992, -16724941, -16724890, -16724839, -16724788, -16724737, -16711936, -16711885, -16711834, -16711783, -16711732, -16711681, -13434880, -13434829, -13434778, -13434727, -13434676, -13434625, -13421824, -13421773, -13421722, -13421671, -13421620, -13421569, -13408768, -13408717, -13408666, -13408615, -13408564, -13408513, -13395712, -13395661, -13395610, -13395559, -13395508, -13395457, -13382656, -13382605, -13382554, -13382503, -13382452, -13382401, -13369600, -13369549, -13369498, -13369447, -13369396, -13369345, -10092544, -10092493, -10092442, -10092391, -10092340, -10092289, -10079488, -10079437, -10079386, -10079335, -10079284, -10079233, -10066432, -10066381, -10066330, -10066279, -10066228, -10066177, -10053376, -10053325, -10053274, -10053223, -10053172, -10053121, -10040320, -10040269, -10040218, -10040167, -10040116, -10040065, -10027264, -10027213, -10027162, -10027111, -10027060, -10027009, -6750208, -6750157, -6750106, -6750055, -6750004, -6749953, -6737152, -6737101, -6737050, -6736999, -6736948, -6736897, -6724096, -6724045, -6723994, -6723943, -6723892, -6723841, -6711040, -6710989, -6710938, -6710887, -6710836, -6710785, -6697984, -6697933, -6697882, -6697831, -6697780, -6697729, -6684928, -6684877, -6684826, -6684775, -6684724, -6684673, -3407872, -3407821, -3407770, -3407719, -3407668, -3407617, -3394816, -3394765, -3394714, -3394663, -3394612, -3394561, -3381760, -3381709, -3381658, -3381607, -3381556, -3381505, -3368704, -3368653, -3368602, -3368551, -3368500, -3368449, -3355648, -3355597, -3355546, -3355495, -3355444, -3355393, -3342592, -3342541, -3342490, -3342439, -3342388, -3342337, -65536, -65485, -65434, -65383, -65332, -65281, -52480, -52429, -52378, -52327, -52276, -52225, -39424, -39373, -39322, -39271, -39220, -39169, -26368, -26317, -26266, -26215, -26164, -26113, -13312, -13261, -13210, -13159, -13108, -13057, -256, -205, -154, -103, -52, -1 } }
    };

    [Theory]
    [MemberData(nameof(Palette_TestData))]
    public void Palette_Get_ReturnsExpected(PixelFormat pixelFormat, int[] expectedEntries)
    {
        using Bitmap bitmap = new(1, 1, pixelFormat);
        Assert.Equal(expectedEntries, bitmap.Palette.Entries.Select(c => c.ToArgb()));
    }

    [Fact]
    public void Palette_SetNull_ThrowsNullReferenceException()
    {
        using Bitmap bitmap = new(1, 1);
        Assert.Throws<NullReferenceException>(() => bitmap.Palette = null);
    }

    [Fact]
    public void Palette_Disposed_ThrowsArgumentException()
    {
        Bitmap bitmap = new(1, 1);
        ColorPalette palette = bitmap.Palette;
        bitmap.Dispose();

        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.Palette);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.Palette = palette);
        AssertExtensions.Throws<ArgumentException>(null, () => bitmap.Size);
    }

    [Fact(Skip = "Condition not met", SkipType = typeof(PlatformDetection), SkipUnless = nameof(PlatformDetection.IsNotArm64Process))] // [ActiveIssue("https://github.com/dotnet/winforms/issues/8817")]
    public void LockBits_Marshalling_Success()
    {
        Color red = Color.FromArgb(Color.Red.ToArgb());
        Color blue = Color.FromArgb(Color.Blue.ToArgb());

        using (Bitmap bitmap = new(1, 1, PixelFormat.Format32bppRgb))
        {
            bitmap.SetPixel(0, 0, red);
            Color pixelColor = bitmap.GetPixel(0, 0);
            Assert.Equal(red, pixelColor);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                int pixelValue = Marshal.ReadByte(data.Scan0, 0);
                pixelValue |= Marshal.ReadByte(data.Scan0, 1) << 8;
                pixelValue |= Marshal.ReadByte(data.Scan0, 2) << 16;
                pixelValue |= Marshal.ReadByte(data.Scan0, 3) << 24;

                pixelColor = Color.FromArgb(pixelValue);
                // Disregard alpha information in the test
                pixelColor = Color.FromArgb(red.A, pixelColor.R, pixelColor.G, pixelColor.B);
                Assert.Equal(red, pixelColor);

                // write blue but we're locked in read-only...
                Marshal.WriteByte(data.Scan0, 0, blue.B);
                Marshal.WriteByte(data.Scan0, 1, blue.G);
                Marshal.WriteByte(data.Scan0, 2, blue.R);
                Marshal.WriteByte(data.Scan0, 3, blue.A);
            }
            finally
            {
                bitmap.UnlockBits(data);
                pixelColor = bitmap.GetPixel(0, 0);
                // Disregard alpha information in the test
                pixelColor = Color.FromArgb(red.A, pixelColor.R, pixelColor.G, pixelColor.B);
                // ...so we still read red after unlocking
                Assert.Equal(red, pixelColor);
            }

            data = bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            try
            {
                // write blue
                Marshal.WriteByte(data.Scan0, 0, blue.B);
                Marshal.WriteByte(data.Scan0, 1, blue.G);
                Marshal.WriteByte(data.Scan0, 2, blue.R);
                Marshal.WriteByte(data.Scan0, 3, blue.A);
            }
            finally
            {
                bitmap.UnlockBits(data);
                pixelColor = bitmap.GetPixel(0, 0);
                // Disregard alpha information in the test
                pixelColor = Color.FromArgb(blue.A, pixelColor.R, pixelColor.G, pixelColor.B);
                // read blue
                Assert.Equal(blue, pixelColor);
            }
        }

        using (Bitmap bitmap = new(1, 1, PixelFormat.Format32bppArgb))
        {
            bitmap.SetPixel(0, 0, red);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                byte b = Marshal.ReadByte(data.Scan0, 0);
                byte g = Marshal.ReadByte(data.Scan0, 1);
                byte r = Marshal.ReadByte(data.Scan0, 2);
                Assert.Equal(red, Color.FromArgb(red.A, r, g, b));
                // write blue but we're locked in read-only...
                Marshal.WriteByte(data.Scan0, 0, blue.B);
                Marshal.WriteByte(data.Scan0, 1, blue.G);
                Marshal.WriteByte(data.Scan0, 2, blue.R);
            }
            finally
            {
                bitmap.UnlockBits(data);
                // ...so we still read red after unlocking
                Assert.Equal(red, bitmap.GetPixel(0, 0));
            }

            data = bitmap.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            try
            {
                // write blue
                Marshal.WriteByte(data.Scan0, 0, blue.B);
                Marshal.WriteByte(data.Scan0, 1, blue.G);
                Marshal.WriteByte(data.Scan0, 2, blue.R);
            }
            finally
            {
                bitmap.UnlockBits(data);
                // read blue
                Assert.Equal(blue, bitmap.GetPixel(0, 0));
            }
        }
    }

    [Fact]
    public void FromNonSeekableStream()
    {
        string path = GetTestFilePath();
        using (Bitmap bitmap = new(100, 100))
        {
            bitmap.Save(path, ImageFormat.Png);
        }

        using (FileStream stream = new(path, FileMode.Open))
        {
            using Bitmap bitmap = new(new TestStream(stream, canSeek: false));
            Assert.Equal(100, bitmap.Height);
            Assert.Equal(100, bitmap.Width);
            Assert.Equal(ImageFormat.Png, bitmap.RawFormat);
        }
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(false, true)]
    public void SaveToRestrictiveStream(bool canRead, bool canSeek)
    {
        using Stream backingStream = new MemoryStream();
        using Stream restrictiveStream = new TestStream(backingStream, canRead, canSeek);
        using (Bitmap bitmap = new(100, 100))
        {
            bitmap.Save(restrictiveStream, ImageFormat.Png);
        }

        backingStream.Position = 0;

        using (Bitmap bitmap = new(backingStream))
        {
            Assert.Equal(100, bitmap.Height);
            Assert.Equal(100, bitmap.Width);
            Assert.Equal(ImageFormat.Png, bitmap.RawFormat);
        }
    }

#if NET9_0_OR_GREATER
    public static TheoryData<PixelFormat, DitherType, PaletteType> Convert_Valid { get; } = new()
    {
        // PaletteType is ignored for non-indexed formats
        { PixelFormat.Format16bppArgb1555, DitherType.None, PaletteType.FixedHalftone8 },
        { PixelFormat.Format16bppRgb555, DitherType.Spiral8x8, PaletteType.FixedHalftone8 },
        { PixelFormat.Format16bppRgb565, DitherType.Ordered8x8, PaletteType.FixedHalftone8 },
        { PixelFormat.Format24bppRgb, DitherType.Ordered4x4, PaletteType.FixedHalftone8 },
        { PixelFormat.Format32bppArgb, DitherType.DualSpiral4x4, PaletteType.FixedHalftone8 },
        { PixelFormat.Format32bppPArgb, DitherType.ErrorDiffusion, PaletteType.FixedHalftone8 },
        { PixelFormat.Format32bppRgb, DitherType.Solid, PaletteType.FixedHalftone8 },
        { PixelFormat.Format16bppArgb1555, DitherType.None, PaletteType.FixedHalftone256 },
        { PixelFormat.Format16bppRgb555, DitherType.DualSpiral8x8, PaletteType.FixedHalftone256 },
        { PixelFormat.Format16bppRgb565, DitherType.None, PaletteType.FixedHalftone256 },
        { PixelFormat.Format24bppRgb, DitherType.None, PaletteType.FixedHalftone256 },
        { PixelFormat.Format32bppArgb, DitherType.None, PaletteType.FixedHalftone256 },
        { PixelFormat.Format32bppPArgb, DitherType.None, PaletteType.FixedHalftone256 },
        { PixelFormat.Format32bppRgb, DitherType.None, PaletteType.FixedHalftone256 },
        { PixelFormat.Format16bppRgb565, DitherType.None, (PaletteType)(-1) },
    };

    public static TheoryData<PixelFormat, DitherType, PaletteType> Convert_InvalidArgument { get; } = new()
    {
        // Indexed formats MUST always have a specified ColorPalette
        { PixelFormat.Format1bppIndexed, (DitherType)(-1), PaletteType.FixedHalftone256 },
        { PixelFormat.Format1bppIndexed, DitherType.None, (PaletteType)(-1) },
        { PixelFormat.Format1bppIndexed, (DitherType)(-1), (PaletteType)(-1) },
        { PixelFormat.Format1bppIndexed, DitherType.None, PaletteType.FixedHalftone256 },
        { PixelFormat.Format1bppIndexed, DitherType.ErrorDiffusion, PaletteType.FixedHalftone8 },
        { PixelFormat.Format4bppIndexed, DitherType.Solid, PaletteType.FixedHalftone64 },
        { PixelFormat.Format4bppIndexed, DitherType.None, PaletteType.FixedHalftone8 },
        { PixelFormat.Format4bppIndexed, DitherType.ErrorDiffusion, PaletteType.FixedHalftone8 },
        { PixelFormat.Format8bppIndexed, DitherType.None, PaletteType.FixedHalftone256 },
        { PixelFormat.Format8bppIndexed, DitherType.Solid, PaletteType.FixedHalftone27 },
        // Format16bppGrayScale is not supported for conversion
        { PixelFormat.Format16bppGrayScale, DitherType.None, PaletteType.FixedHalftone256 },
        { PixelFormat.Format16bppGrayScale, DitherType.ErrorDiffusion, PaletteType.FixedHalftone8 },
        { PixelFormat.Format16bppGrayScale, DitherType.None, PaletteType.FixedBlackAndWhite },
        { PixelFormat.Format16bppGrayScale, DitherType.Solid, PaletteType.FixedBlackAndWhite },
        { PixelFormat.Format16bppRgb565, (DitherType)(-1), PaletteType.FixedHalftone256 },
        { PixelFormat.Format16bppRgb565, (DitherType)(-1), (PaletteType)(-1) },
    };

    [Theory]
    [MemberData(nameof(Convert_Valid))]
    public void Bitmap_Convert_BasicPixelFormat(PixelFormat format, DitherType dither, PaletteType palette)
    {
        using Bitmap bitmap = new(1, 1);
        bitmap.ConvertFormat(format, dither, palette);
        bitmap.PixelFormat.Should().Be(format);
    }

    [Theory]
    [MemberData(nameof(Convert_InvalidArgument))]
    public void Bitmap_Convert_ArgumentException(PixelFormat format, DitherType dither, PaletteType palette)
    {
        using Bitmap bitmap = new(1, 1);
        bitmap.Invoking(b => b.ConvertFormat(format, dither, palette)).Should().Throw<ArgumentException>();
    }

    public static TheoryData<PixelFormat> AllValidPixelFormats { get; } = new()
    {
        PixelFormat.Format16bppArgb1555,
        PixelFormat.Format16bppRgb555,
        PixelFormat.Format16bppRgb565,
        PixelFormat.Format24bppRgb,
        PixelFormat.Format32bppArgb,
        PixelFormat.Format32bppPArgb,
        PixelFormat.Format32bppRgb,
        PixelFormat.Format48bppRgb,
        PixelFormat.Format64bppArgb,
        PixelFormat.Format64bppPArgb,
        PixelFormat.Format8bppIndexed,
        PixelFormat.Format1bppIndexed,
        PixelFormat.Format4bppIndexed
    };

    [Theory]
    [MemberData(nameof(AllValidPixelFormats))]
    public void Bitmap_Convert_SingleArgument(PixelFormat format)
    {
        using Bitmap bitmap = new(1, 1);
        bitmap.ConvertFormat(format);
        bitmap.PixelFormat.Should().Be(format);
    }
#endif

    private class TestStream : Stream
    {
        private readonly Stream _stream;
        private readonly bool _canRead;
        private readonly bool _canSeek;

        public TestStream(Stream stream, bool canRead = true, bool canSeek = true)
        {
            _stream = stream;
            _canRead = canRead;
            _canSeek = canSeek;
        }

        public override bool CanRead => _canRead && _stream.CanRead;
        public override bool CanSeek => _canSeek && _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;
        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = _canSeek ? value : throw new NotSupportedException();
        }

        public override void Flush() => _stream.Flush();
        public override int Read(byte[] buffer, int offset, int count) => _canRead ? _stream.Read(buffer, offset, count) : throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);
        public override void SetLength(long value) => _stream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);
    }
}
