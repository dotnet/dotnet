// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.DisableRuntimeMarshalling]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
[assembly: System.Runtime.Versioning.SupportedOSPlatform("windows6.1")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Drawing.Common")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.23.53105")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.0+e4ede9b8979b9d2b1b1d4383f30a791414f0625b")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Drawing.Common")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/winforms")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.Color))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.ColorTranslator))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.KnownColor))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.Point))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.PointF))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.Rectangle))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.RectangleF))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.Size))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.SizeF))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.SystemColors))]
namespace System.Drawing
{
    public sealed partial class Bitmap : Image
    {
        public Bitmap(Image original, Size newSize) { }

        public Bitmap(Image original, int width, int height) { }

        public Bitmap(Image original) { }

        public Bitmap(int width, int height, Graphics g) { }

        public Bitmap(int width, int height, Imaging.PixelFormat format) { }

        public Bitmap(int width, int height, int stride, Imaging.PixelFormat format, nint scan0) { }

        public Bitmap(int width, int height) { }

        public Bitmap(IO.Stream stream, bool useIcm) { }

        public Bitmap(IO.Stream stream) { }

        public Bitmap(string filename, bool useIcm) { }

        public Bitmap(string filename) { }

        public Bitmap(Type type, string resource) { }

        public Bitmap Clone(Rectangle rect, Imaging.PixelFormat format) { throw null; }

        public Bitmap Clone(RectangleF rect, Imaging.PixelFormat format) { throw null; }

        public static Bitmap FromHicon(nint hicon) { throw null; }

        public static Bitmap FromResource(nint hinstance, string bitmapName) { throw null; }

        public nint GetHbitmap() { throw null; }

        public nint GetHbitmap(Color background) { throw null; }

        public nint GetHicon() { throw null; }

        public Color GetPixel(int x, int y) { throw null; }

        public Imaging.BitmapData LockBits(Rectangle rect, Imaging.ImageLockMode flags, Imaging.PixelFormat format, Imaging.BitmapData bitmapData) { throw null; }

        public Imaging.BitmapData LockBits(Rectangle rect, Imaging.ImageLockMode flags, Imaging.PixelFormat format) { throw null; }

        public void MakeTransparent() { }

        public void MakeTransparent(Color transparentColor) { }

        public void SetPixel(int x, int y, Color color) { }

        public void SetResolution(float xDpi, float yDpi) { }

        public void UnlockBits(Imaging.BitmapData bitmapdata) { }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public partial class BitmapSuffixInSameAssemblyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public partial class BitmapSuffixInSatelliteAssemblyAttribute : Attribute
    {
    }

    public abstract partial class Brush : MarshalByRefObject, ICloneable, IDisposable
    {
        public abstract object Clone();
        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~Brush() {
        }

        protected internal void SetNativeBrush(nint brush) { }
    }

    public static partial class Brushes
    {
        public static Brush AliceBlue { get { throw null; } }

        public static Brush AntiqueWhite { get { throw null; } }

        public static Brush Aqua { get { throw null; } }

        public static Brush Aquamarine { get { throw null; } }

        public static Brush Azure { get { throw null; } }

        public static Brush Beige { get { throw null; } }

        public static Brush Bisque { get { throw null; } }

        public static Brush Black { get { throw null; } }

        public static Brush BlanchedAlmond { get { throw null; } }

        public static Brush Blue { get { throw null; } }

        public static Brush BlueViolet { get { throw null; } }

        public static Brush Brown { get { throw null; } }

        public static Brush BurlyWood { get { throw null; } }

        public static Brush CadetBlue { get { throw null; } }

        public static Brush Chartreuse { get { throw null; } }

        public static Brush Chocolate { get { throw null; } }

        public static Brush Coral { get { throw null; } }

        public static Brush CornflowerBlue { get { throw null; } }

        public static Brush Cornsilk { get { throw null; } }

        public static Brush Crimson { get { throw null; } }

        public static Brush Cyan { get { throw null; } }

        public static Brush DarkBlue { get { throw null; } }

        public static Brush DarkCyan { get { throw null; } }

        public static Brush DarkGoldenrod { get { throw null; } }

        public static Brush DarkGray { get { throw null; } }

        public static Brush DarkGreen { get { throw null; } }

        public static Brush DarkKhaki { get { throw null; } }

        public static Brush DarkMagenta { get { throw null; } }

        public static Brush DarkOliveGreen { get { throw null; } }

        public static Brush DarkOrange { get { throw null; } }

        public static Brush DarkOrchid { get { throw null; } }

        public static Brush DarkRed { get { throw null; } }

        public static Brush DarkSalmon { get { throw null; } }

        public static Brush DarkSeaGreen { get { throw null; } }

        public static Brush DarkSlateBlue { get { throw null; } }

        public static Brush DarkSlateGray { get { throw null; } }

        public static Brush DarkTurquoise { get { throw null; } }

        public static Brush DarkViolet { get { throw null; } }

        public static Brush DeepPink { get { throw null; } }

        public static Brush DeepSkyBlue { get { throw null; } }

        public static Brush DimGray { get { throw null; } }

        public static Brush DodgerBlue { get { throw null; } }

        public static Brush Firebrick { get { throw null; } }

        public static Brush FloralWhite { get { throw null; } }

        public static Brush ForestGreen { get { throw null; } }

        public static Brush Fuchsia { get { throw null; } }

        public static Brush Gainsboro { get { throw null; } }

        public static Brush GhostWhite { get { throw null; } }

        public static Brush Gold { get { throw null; } }

        public static Brush Goldenrod { get { throw null; } }

        public static Brush Gray { get { throw null; } }

        public static Brush Green { get { throw null; } }

        public static Brush GreenYellow { get { throw null; } }

        public static Brush Honeydew { get { throw null; } }

        public static Brush HotPink { get { throw null; } }

        public static Brush IndianRed { get { throw null; } }

        public static Brush Indigo { get { throw null; } }

        public static Brush Ivory { get { throw null; } }

        public static Brush Khaki { get { throw null; } }

        public static Brush Lavender { get { throw null; } }

        public static Brush LavenderBlush { get { throw null; } }

        public static Brush LawnGreen { get { throw null; } }

        public static Brush LemonChiffon { get { throw null; } }

        public static Brush LightBlue { get { throw null; } }

        public static Brush LightCoral { get { throw null; } }

        public static Brush LightCyan { get { throw null; } }

        public static Brush LightGoldenrodYellow { get { throw null; } }

        public static Brush LightGray { get { throw null; } }

        public static Brush LightGreen { get { throw null; } }

        public static Brush LightPink { get { throw null; } }

        public static Brush LightSalmon { get { throw null; } }

        public static Brush LightSeaGreen { get { throw null; } }

        public static Brush LightSkyBlue { get { throw null; } }

        public static Brush LightSlateGray { get { throw null; } }

        public static Brush LightSteelBlue { get { throw null; } }

        public static Brush LightYellow { get { throw null; } }

        public static Brush Lime { get { throw null; } }

        public static Brush LimeGreen { get { throw null; } }

        public static Brush Linen { get { throw null; } }

        public static Brush Magenta { get { throw null; } }

        public static Brush Maroon { get { throw null; } }

        public static Brush MediumAquamarine { get { throw null; } }

        public static Brush MediumBlue { get { throw null; } }

        public static Brush MediumOrchid { get { throw null; } }

        public static Brush MediumPurple { get { throw null; } }

        public static Brush MediumSeaGreen { get { throw null; } }

        public static Brush MediumSlateBlue { get { throw null; } }

        public static Brush MediumSpringGreen { get { throw null; } }

        public static Brush MediumTurquoise { get { throw null; } }

        public static Brush MediumVioletRed { get { throw null; } }

        public static Brush MidnightBlue { get { throw null; } }

        public static Brush MintCream { get { throw null; } }

        public static Brush MistyRose { get { throw null; } }

        public static Brush Moccasin { get { throw null; } }

        public static Brush NavajoWhite { get { throw null; } }

        public static Brush Navy { get { throw null; } }

        public static Brush OldLace { get { throw null; } }

        public static Brush Olive { get { throw null; } }

        public static Brush OliveDrab { get { throw null; } }

        public static Brush Orange { get { throw null; } }

        public static Brush OrangeRed { get { throw null; } }

        public static Brush Orchid { get { throw null; } }

        public static Brush PaleGoldenrod { get { throw null; } }

        public static Brush PaleGreen { get { throw null; } }

        public static Brush PaleTurquoise { get { throw null; } }

        public static Brush PaleVioletRed { get { throw null; } }

        public static Brush PapayaWhip { get { throw null; } }

        public static Brush PeachPuff { get { throw null; } }

        public static Brush Peru { get { throw null; } }

        public static Brush Pink { get { throw null; } }

        public static Brush Plum { get { throw null; } }

        public static Brush PowderBlue { get { throw null; } }

        public static Brush Purple { get { throw null; } }

        public static Brush Red { get { throw null; } }

        public static Brush RosyBrown { get { throw null; } }

        public static Brush RoyalBlue { get { throw null; } }

        public static Brush SaddleBrown { get { throw null; } }

        public static Brush Salmon { get { throw null; } }

        public static Brush SandyBrown { get { throw null; } }

        public static Brush SeaGreen { get { throw null; } }

        public static Brush SeaShell { get { throw null; } }

        public static Brush Sienna { get { throw null; } }

        public static Brush Silver { get { throw null; } }

        public static Brush SkyBlue { get { throw null; } }

        public static Brush SlateBlue { get { throw null; } }

        public static Brush SlateGray { get { throw null; } }

        public static Brush Snow { get { throw null; } }

        public static Brush SpringGreen { get { throw null; } }

        public static Brush SteelBlue { get { throw null; } }

        public static Brush Tan { get { throw null; } }

        public static Brush Teal { get { throw null; } }

        public static Brush Thistle { get { throw null; } }

        public static Brush Tomato { get { throw null; } }

        public static Brush Transparent { get { throw null; } }

        public static Brush Turquoise { get { throw null; } }

        public static Brush Violet { get { throw null; } }

        public static Brush Wheat { get { throw null; } }

        public static Brush White { get { throw null; } }

        public static Brush WhiteSmoke { get { throw null; } }

        public static Brush Yellow { get { throw null; } }

        public static Brush YellowGreen { get { throw null; } }
    }

    public sealed partial class BufferedGraphics : IDisposable
    {
        internal BufferedGraphics() { }

        public Graphics Graphics { get { throw null; } }

        public void Dispose() { }

        public void Render() { }

        public void Render(Graphics? target) { }

        public void Render(nint targetDC) { }
    }

    public sealed partial class BufferedGraphicsContext : IDisposable
    {
        public Size MaximumBuffer { get { throw null; } set { } }

        public BufferedGraphics Allocate(Graphics targetGraphics, Rectangle targetRectangle) { throw null; }

        public BufferedGraphics Allocate(nint targetDC, Rectangle targetRectangle) { throw null; }

        public void Dispose() { }

        ~BufferedGraphicsContext() {
        }

        public void Invalidate() { }
    }

    public static partial class BufferedGraphicsManager
    {
        public static BufferedGraphicsContext Current { get { throw null; } }
    }

    public partial struct CharacterRange : IEquatable<CharacterRange>
    {
        private int _dummyPrimitive;
        public CharacterRange(int First, int Length) { }

        public int First { get { throw null; } set { } }

        public int Length { get { throw null; } set { } }

        public readonly bool Equals(CharacterRange other) { throw null; }

        public override readonly bool Equals(object? obj) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(CharacterRange cr1, CharacterRange cr2) { throw null; }

        public static bool operator !=(CharacterRange cr1, CharacterRange cr2) { throw null; }
    }

    public enum ContentAlignment
    {
        TopLeft = 1,
        TopCenter = 2,
        TopRight = 4,
        MiddleLeft = 16,
        MiddleCenter = 32,
        MiddleRight = 64,
        BottomLeft = 256,
        BottomCenter = 512,
        BottomRight = 1024
    }

    public enum CopyPixelOperation
    {
        NoMirrorBitmap = int.MinValue,
        Blackness = 66,
        NotSourceErase = 1114278,
        NotSourceCopy = 3342344,
        SourceErase = 4457256,
        DestinationInvert = 5570569,
        PatInvert = 5898313,
        SourceInvert = 6684742,
        SourceAnd = 8913094,
        MergePaint = 12255782,
        MergeCopy = 12583114,
        SourceCopy = 13369376,
        SourcePaint = 15597702,
        PatCopy = 15728673,
        PatPaint = 16452105,
        Whiteness = 16711778,
        CaptureBlt = 1073741824
    }

    public sealed partial class Font : MarshalByRefObject, ICloneable, IDisposable, Runtime.Serialization.ISerializable
    {
        public Font(Font prototype, FontStyle newStyle) { }

        public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont) { }

        public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet) { }

        public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit) { }

        public Font(FontFamily family, float emSize, FontStyle style) { }

        public Font(FontFamily family, float emSize, GraphicsUnit unit) { }

        public Font(FontFamily family, float emSize) { }

        public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont) { }

        public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet) { }

        public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit) { }

        public Font(string familyName, float emSize, FontStyle style) { }

        public Font(string familyName, float emSize, GraphicsUnit unit) { }

        public Font(string familyName, float emSize) { }

        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool Bold { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public FontFamily FontFamily { get { throw null; } }

        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public byte GdiCharSet { get { throw null; } }

        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool GdiVerticalFont { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public int Height { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public bool IsSystemFont { get { throw null; } }

        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool Italic { get { throw null; } }

        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string Name { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public string? OriginalFontName { get { throw null; } }

        public float Size { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public float SizeInPoints { get { throw null; } }

        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool Strikeout { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public FontStyle Style { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public string SystemFontName { get { throw null; } }

        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool Underline { get { throw null; } }

        public GraphicsUnit Unit { get { throw null; } }

        public object Clone() { throw null; }

        public void Dispose() { }

        public override bool Equals(object? obj) { throw null; }

        ~Font() {
        }

        public static Font FromHdc(nint hdc) { throw null; }

        public static Font FromHfont(nint hfont) { throw null; }

        public static Font FromLogFont(in Interop.LOGFONT logFont, nint hdc) { throw null; }

        public static Font FromLogFont(in Interop.LOGFONT logFont) { throw null; }

        public static Font FromLogFont(object lf, nint hdc) { throw null; }

        public static Font FromLogFont(object lf) { throw null; }

        public override int GetHashCode() { throw null; }

        public float GetHeight() { throw null; }

        public float GetHeight(Graphics graphics) { throw null; }

        public float GetHeight(float dpi) { throw null; }

        void Runtime.Serialization.ISerializable.GetObjectData(Runtime.Serialization.SerializationInfo si, Runtime.Serialization.StreamingContext context) { }

        public nint ToHfont() { throw null; }

        public void ToLogFont(out Interop.LOGFONT logFont, Graphics graphics) { throw null; }

        public void ToLogFont(out Interop.LOGFONT logFont) { throw null; }

        public void ToLogFont(object logFont, Graphics graphics) { }

        public void ToLogFont(object logFont) { }

        public override string ToString() { throw null; }
    }

    public partial class FontConverter : ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext? context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext? context, Type? destinationType) { throw null; }

        public override object? ConvertFrom(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object value) { throw null; }

        public override object? ConvertTo(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object? value, Type destinationType) { throw null; }

        public override object CreateInstance(ComponentModel.ITypeDescriptorContext? context, Collections.IDictionary propertyValues) { throw null; }

        public override bool GetCreateInstanceSupported(ComponentModel.ITypeDescriptorContext? context) { throw null; }

        [Diagnostics.CodeAnalysis.RequiresUnreferencedCode("The Type of value cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
        public override ComponentModel.PropertyDescriptorCollection? GetProperties(ComponentModel.ITypeDescriptorContext? context, object value, Attribute[]? attributes) { throw null; }

        public override bool GetPropertiesSupported(ComponentModel.ITypeDescriptorContext? context) { throw null; }

        public sealed partial class FontNameConverter : ComponentModel.TypeConverter, IDisposable
        {
            public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext? context, Type sourceType) { throw null; }

            public override object? ConvertFrom(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object value) { throw null; }

            public override StandardValuesCollection GetStandardValues(ComponentModel.ITypeDescriptorContext? context) { throw null; }

            public override bool GetStandardValuesExclusive(ComponentModel.ITypeDescriptorContext? context) { throw null; }

            public override bool GetStandardValuesSupported(ComponentModel.ITypeDescriptorContext? context) { throw null; }

            void IDisposable.Dispose() { }
        }

        public partial class FontUnitConverter : ComponentModel.EnumConverter
        {
            public FontUnitConverter() : base(default!) { }

            public override StandardValuesCollection GetStandardValues(ComponentModel.ITypeDescriptorContext? context) { throw null; }
        }
    }

    public sealed partial class FontFamily : MarshalByRefObject, IDisposable
    {
        public FontFamily(Text.GenericFontFamilies genericFamily) { }

        public FontFamily(string name, Text.FontCollection? fontCollection) { }

        public FontFamily(string name) { }

        public static FontFamily[] Families { get { throw null; } }

        public static FontFamily GenericMonospace { get { throw null; } }

        public static FontFamily GenericSansSerif { get { throw null; } }

        public static FontFamily GenericSerif { get { throw null; } }

        public string Name { get { throw null; } }

        public void Dispose() { }

        public override bool Equals(object? obj) { throw null; }

        ~FontFamily() {
        }

        public int GetCellAscent(FontStyle style) { throw null; }

        public int GetCellDescent(FontStyle style) { throw null; }

        public int GetEmHeight(FontStyle style) { throw null; }

        [Obsolete("FontFamily.GetFamilies has been deprecated. Use Families instead.")]
        public static FontFamily[] GetFamilies(Graphics graphics) { throw null; }

        public override int GetHashCode() { throw null; }

        public int GetLineSpacing(FontStyle style) { throw null; }

        public string GetName(int language) { throw null; }

        public bool IsStyleAvailable(FontStyle style) { throw null; }

        public override string ToString() { throw null; }
    }

    [Flags]
    public enum FontStyle
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Strikeout = 8
    }

    public sealed partial class Graphics : MarshalByRefObject, IDisposable, IDeviceContext
    {
        internal Graphics() { }

        public Region Clip { get { throw null; } set { } }

        public RectangleF ClipBounds { get { throw null; } }

        public Drawing2D.CompositingMode CompositingMode { get { throw null; } set { } }

        public Drawing2D.CompositingQuality CompositingQuality { get { throw null; } set { } }

        public float DpiX { get { throw null; } }

        public float DpiY { get { throw null; } }

        public Drawing2D.InterpolationMode InterpolationMode { get { throw null; } set { } }

        public bool IsClipEmpty { get { throw null; } }

        public bool IsVisibleClipEmpty { get { throw null; } }

        public float PageScale { get { throw null; } set { } }

        public GraphicsUnit PageUnit { get { throw null; } set { } }

        public Drawing2D.PixelOffsetMode PixelOffsetMode { get { throw null; } set { } }

        public Point RenderingOrigin { get { throw null; } set { } }

        public Drawing2D.SmoothingMode SmoothingMode { get { throw null; } set { } }

        public int TextContrast { get { throw null; } set { } }

        public Text.TextRenderingHint TextRenderingHint { get { throw null; } set { } }

        public Drawing2D.Matrix Transform { get { throw null; } set { } }

        public Numerics.Matrix3x2 TransformElements { get { throw null; } set { } }

        public RectangleF VisibleClipBounds { get { throw null; } }

        public void AddMetafileComment(byte[] data) { }

        public Drawing2D.GraphicsContainer BeginContainer() { throw null; }

        public Drawing2D.GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit) { throw null; }

        public Drawing2D.GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit) { throw null; }

        public void Clear(Color color) { }

        public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize, CopyPixelOperation copyPixelOperation) { }

        public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize) { }

        public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation) { }

        public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize) { }

        public void Dispose() { }

        public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle) { }

        public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle) { }

        public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle) { }

        public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle) { }

        public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4) { }

        public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4) { }

        public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) { }

        public void DrawBeziers(Pen pen, Point[] points) { }

        public void DrawBeziers(Pen pen, PointF[] points) { }

        public void DrawCachedBitmap(Imaging.CachedBitmap cachedBitmap, int x, int y) { }

        public void DrawClosedCurve(Pen pen, Point[] points, float tension, Drawing2D.FillMode fillmode) { }

        public void DrawClosedCurve(Pen pen, Point[] points) { }

        public void DrawClosedCurve(Pen pen, PointF[] points, float tension, Drawing2D.FillMode fillmode) { }

        public void DrawClosedCurve(Pen pen, PointF[] points) { }

        public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension) { }

        public void DrawCurve(Pen pen, Point[] points, float tension) { }

        public void DrawCurve(Pen pen, Point[] points) { }

        public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension) { }

        public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments) { }

        public void DrawCurve(Pen pen, PointF[] points, float tension) { }

        public void DrawCurve(Pen pen, PointF[] points) { }

        public void DrawEllipse(Pen pen, Rectangle rect) { }

        public void DrawEllipse(Pen pen, RectangleF rect) { }

        public void DrawEllipse(Pen pen, int x, int y, int width, int height) { }

        public void DrawEllipse(Pen pen, float x, float y, float width, float height) { }

        public void DrawIcon(Icon icon, Rectangle targetRect) { }

        public void DrawIcon(Icon icon, int x, int y) { }

        public void DrawIconUnstretched(Icon icon, Rectangle targetRect) { }

        public void DrawImage(Image image, Point point) { }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttr, DrawImageAbort? callback, int callbackData) { }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttr, DrawImageAbort? callback) { }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttr) { }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit) { }

        public void DrawImage(Image image, Point[] destPoints) { }

        public void DrawImage(Image image, PointF point) { }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttr, DrawImageAbort? callback, int callbackData) { }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttr, DrawImageAbort? callback) { }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttr) { }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit) { }

        public void DrawImage(Image image, PointF[] destPoints) { }

        public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit) { }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttrs, DrawImageAbort? callback, nint callbackData) { }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttr, DrawImageAbort? callback) { }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttr) { }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit) { }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttrs, DrawImageAbort? callback, nint callbackData) { }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttrs, DrawImageAbort? callback) { }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, Imaging.ImageAttributes? imageAttrs) { }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit) { }

        public void DrawImage(Image image, Rectangle rect) { }

        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit) { }

        public void DrawImage(Image image, RectangleF rect) { }

        public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit) { }

        public void DrawImage(Image image, int x, int y, int width, int height) { }

        public void DrawImage(Image image, int x, int y) { }

        public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit) { }

        public void DrawImage(Image image, float x, float y, float width, float height) { }

        public void DrawImage(Image image, float x, float y) { }

        public void DrawImageUnscaled(Image image, Point point) { }

        public void DrawImageUnscaled(Image image, Rectangle rect) { }

        public void DrawImageUnscaled(Image image, int x, int y, int width, int height) { }

        public void DrawImageUnscaled(Image image, int x, int y) { }

        public void DrawImageUnscaledAndClipped(Image image, Rectangle rect) { }

        public void DrawLine(Pen pen, Point pt1, Point pt2) { }

        public void DrawLine(Pen pen, PointF pt1, PointF pt2) { }

        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2) { }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2) { }

        public void DrawLines(Pen pen, Point[] points) { }

        public void DrawLines(Pen pen, PointF[] points) { }

        public void DrawPath(Pen pen, Drawing2D.GraphicsPath path) { }

        public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle) { }

        public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle) { }

        public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle) { }

        public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle) { }

        public void DrawPolygon(Pen pen, Point[] points) { }

        public void DrawPolygon(Pen pen, PointF[] points) { }

        public void DrawRectangle(Pen pen, Rectangle rect) { }

        public void DrawRectangle(Pen pen, RectangleF rect) { }

        public void DrawRectangle(Pen pen, int x, int y, int width, int height) { }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height) { }

        public void DrawRectangles(Pen pen, Rectangle[] rects) { }

        public void DrawRectangles(Pen pen, RectangleF[] rects) { }

        public void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, PointF point, StringFormat? format) { }

        public void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, PointF point) { }

        public void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat? format) { }

        public void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, RectangleF layoutRectangle) { }

        public void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, float x, float y, StringFormat? format) { }

        public void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, float x, float y) { }

        public void DrawString(string? s, Font font, Brush brush, PointF point, StringFormat? format) { }

        public void DrawString(string? s, Font font, Brush brush, PointF point) { }

        public void DrawString(string? s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat? format) { }

        public void DrawString(string? s, Font font, Brush brush, RectangleF layoutRectangle) { }

        public void DrawString(string? s, Font font, Brush brush, float x, float y, StringFormat? format) { }

        public void DrawString(string? s, Font font, Brush brush, float x, float y) { }

        public void EndContainer(Drawing2D.GraphicsContainer container) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point destPoint, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point destPoint, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point destPoint, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF destPoint, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF destPoint, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF destPoint, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, nint callbackData, Imaging.ImageAttributes? imageAttr) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, nint callbackData) { }

        public void EnumerateMetafile(Imaging.Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback) { }

        public void ExcludeClip(Rectangle rect) { }

        public void ExcludeClip(Region region) { }

        public void FillClosedCurve(Brush brush, Point[] points, Drawing2D.FillMode fillmode, float tension) { }

        public void FillClosedCurve(Brush brush, Point[] points, Drawing2D.FillMode fillmode) { }

        public void FillClosedCurve(Brush brush, Point[] points) { }

        public void FillClosedCurve(Brush brush, PointF[] points, Drawing2D.FillMode fillmode, float tension) { }

        public void FillClosedCurve(Brush brush, PointF[] points, Drawing2D.FillMode fillmode) { }

        public void FillClosedCurve(Brush brush, PointF[] points) { }

        public void FillEllipse(Brush brush, Rectangle rect) { }

        public void FillEllipse(Brush brush, RectangleF rect) { }

        public void FillEllipse(Brush brush, int x, int y, int width, int height) { }

        public void FillEllipse(Brush brush, float x, float y, float width, float height) { }

        public void FillPath(Brush brush, Drawing2D.GraphicsPath path) { }

        public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle) { }

        public void FillPie(Brush brush, RectangleF rect, float startAngle, float sweepAngle) { }

        public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle) { }

        public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle) { }

        public void FillPolygon(Brush brush, Point[] points, Drawing2D.FillMode fillMode) { }

        public void FillPolygon(Brush brush, Point[] points) { }

        public void FillPolygon(Brush brush, PointF[] points, Drawing2D.FillMode fillMode) { }

        public void FillPolygon(Brush brush, PointF[] points) { }

        public void FillRectangle(Brush brush, Rectangle rect) { }

        public void FillRectangle(Brush brush, RectangleF rect) { }

        public void FillRectangle(Brush brush, int x, int y, int width, int height) { }

        public void FillRectangle(Brush brush, float x, float y, float width, float height) { }

        public void FillRectangles(Brush brush, Rectangle[] rects) { }

        public void FillRectangles(Brush brush, RectangleF[] rects) { }

        public void FillRegion(Brush brush, Region region) { }

        ~Graphics() {
        }

        public void Flush() { }

        public void Flush(Drawing2D.FlushIntention intention) { }

        public static Graphics FromHdc(nint hdc, nint hdevice) { throw null; }

        public static Graphics FromHdc(nint hdc) { throw null; }

        public static Graphics FromHdcInternal(nint hdc) { throw null; }

        public static Graphics FromHwnd(nint hwnd) { throw null; }

        public static Graphics FromHwndInternal(nint hwnd) { throw null; }

        public static Graphics FromImage(Image image) { throw null; }

        [Obsolete("Use the Graphics.GetContextInfo overloads that accept arguments for better performance and fewer allocations.", DiagnosticId = "SYSLIB0016", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        [Runtime.Versioning.SupportedOSPlatform("windows")]
        public object GetContextInfo() { throw null; }

        [Runtime.Versioning.SupportedOSPlatform("windows")]
        public void GetContextInfo(out PointF offset, out Region? clip) { throw null; }

        [Runtime.Versioning.SupportedOSPlatform("windows")]
        public void GetContextInfo(out PointF offset) { throw null; }

        public static nint GetHalftonePalette() { throw null; }

        public nint GetHdc() { throw null; }

        public Color GetNearestColor(Color color) { throw null; }

        public void IntersectClip(Rectangle rect) { }

        public void IntersectClip(RectangleF rect) { }

        public void IntersectClip(Region region) { }

        public bool IsVisible(Point point) { throw null; }

        public bool IsVisible(PointF point) { throw null; }

        public bool IsVisible(Rectangle rect) { throw null; }

        public bool IsVisible(RectangleF rect) { throw null; }

        public bool IsVisible(int x, int y, int width, int height) { throw null; }

        public bool IsVisible(int x, int y) { throw null; }

        public bool IsVisible(float x, float y, float width, float height) { throw null; }

        public bool IsVisible(float x, float y) { throw null; }

        public Region[] MeasureCharacterRanges(ReadOnlySpan<char> text, Font font, RectangleF layoutRect, StringFormat? stringFormat) { throw null; }

        public Region[] MeasureCharacterRanges(string? text, Font font, RectangleF layoutRect, StringFormat? stringFormat) { throw null; }

        public SizeF MeasureString(ReadOnlySpan<char> text, Font font, PointF origin, StringFormat? stringFormat) { throw null; }

        public SizeF MeasureString(ReadOnlySpan<char> text, Font font, SizeF layoutArea, StringFormat? stringFormat, out int charactersFitted, out int linesFilled) { throw null; }

        public SizeF MeasureString(ReadOnlySpan<char> text, Font font, SizeF layoutArea, StringFormat? stringFormat) { throw null; }

        public SizeF MeasureString(ReadOnlySpan<char> text, Font font, SizeF layoutArea) { throw null; }

        public SizeF MeasureString(ReadOnlySpan<char> text, Font font, int width, StringFormat? format) { throw null; }

        public SizeF MeasureString(ReadOnlySpan<char> text, Font font, int width) { throw null; }

        public SizeF MeasureString(ReadOnlySpan<char> text, Font font) { throw null; }

        public SizeF MeasureString(string? text, Font font, PointF origin, StringFormat? stringFormat) { throw null; }

        public SizeF MeasureString(string? text, Font font, SizeF layoutArea, StringFormat? stringFormat, out int charactersFitted, out int linesFilled) { throw null; }

        public SizeF MeasureString(string? text, Font font, SizeF layoutArea, StringFormat? stringFormat) { throw null; }

        public SizeF MeasureString(string? text, Font font, SizeF layoutArea) { throw null; }

        public SizeF MeasureString(string? text, Font font, int width, StringFormat? format) { throw null; }

        public SizeF MeasureString(string? text, Font font, int width) { throw null; }

        public SizeF MeasureString(string? text, Font font) { throw null; }

        public void MultiplyTransform(Drawing2D.Matrix matrix, Drawing2D.MatrixOrder order) { }

        public void MultiplyTransform(Drawing2D.Matrix matrix) { }

        public void ReleaseHdc() { }

        public void ReleaseHdc(nint hdc) { }

        public void ReleaseHdcInternal(nint hdc) { }

        public void ResetClip() { }

        public void ResetTransform() { }

        public void Restore(Drawing2D.GraphicsState gstate) { }

        public void RotateTransform(float angle, Drawing2D.MatrixOrder order) { }

        public void RotateTransform(float angle) { }

        public Drawing2D.GraphicsState Save() { throw null; }

        public void ScaleTransform(float sx, float sy, Drawing2D.MatrixOrder order) { }

        public void ScaleTransform(float sx, float sy) { }

        public void SetClip(Drawing2D.GraphicsPath path, Drawing2D.CombineMode combineMode) { }

        public void SetClip(Drawing2D.GraphicsPath path) { }

        public void SetClip(Graphics g, Drawing2D.CombineMode combineMode) { }

        public void SetClip(Graphics g) { }

        public void SetClip(Rectangle rect, Drawing2D.CombineMode combineMode) { }

        public void SetClip(Rectangle rect) { }

        public void SetClip(RectangleF rect, Drawing2D.CombineMode combineMode) { }

        public void SetClip(RectangleF rect) { }

        public void SetClip(Region region, Drawing2D.CombineMode combineMode) { }

        public void TransformPoints(Drawing2D.CoordinateSpace destSpace, Drawing2D.CoordinateSpace srcSpace, Point[] pts) { }

        public void TransformPoints(Drawing2D.CoordinateSpace destSpace, Drawing2D.CoordinateSpace srcSpace, PointF[] pts) { }

        public void TranslateClip(int dx, int dy) { }

        public void TranslateClip(float dx, float dy) { }

        public void TranslateTransform(float dx, float dy, Drawing2D.MatrixOrder order) { }

        public void TranslateTransform(float dx, float dy) { }

        public delegate bool DrawImageAbort(nint callbackdata);
        public delegate bool EnumerateMetafileProc(Imaging.EmfPlusRecordType recordType, int flags, int dataSize, nint data, Imaging.PlayRecordCallback? callbackData);
    }

    public enum GraphicsUnit
    {
        World = 0,
        Display = 1,
        Pixel = 2,
        Point = 3,
        Inch = 4,
        Document = 5,
        Millimeter = 6
    }

    public sealed partial class Icon : MarshalByRefObject, ICloneable, IDisposable, Runtime.Serialization.ISerializable
    {
        public Icon(Icon original, Size size) { }

        public Icon(Icon original, int width, int height) { }

        public Icon(IO.Stream stream, Size size) { }

        public Icon(IO.Stream stream, int width, int height) { }

        public Icon(IO.Stream stream) { }

        public Icon(string fileName, Size size) { }

        public Icon(string fileName, int width, int height) { }

        public Icon(string fileName) { }

        public Icon(Type type, string resource) { }

        [ComponentModel.Browsable(false)]
        public nint Handle { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public int Height { get { throw null; } }

        public Size Size { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public int Width { get { throw null; } }

        public object Clone() { throw null; }

        public void Dispose() { }

        public static Icon? ExtractAssociatedIcon(string filePath) { throw null; }

        public static Icon? ExtractIcon(string filePath, int id, bool smallIcon = false) { throw null; }

        public static Icon? ExtractIcon(string filePath, int id, int size) { throw null; }

        ~Icon() {
        }

        public static Icon FromHandle(nint handle) { throw null; }

        public void Save(IO.Stream outputStream) { }

        void Runtime.Serialization.ISerializable.GetObjectData(Runtime.Serialization.SerializationInfo si, Runtime.Serialization.StreamingContext context) { }

        public Bitmap ToBitmap() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class IconConverter : ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext? context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext? context, Type? destinationType) { throw null; }

        public override object? ConvertFrom(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object value) { throw null; }

        public override object? ConvertTo(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object? value, Type destinationType) { throw null; }
    }

    public partial interface IDeviceContext : IDisposable
    {
        nint GetHdc();
        void ReleaseHdc();
    }

    [ComponentModel.ImmutableObject(true)]
    public abstract partial class Image : MarshalByRefObject, IDisposable, ICloneable, Runtime.Serialization.ISerializable
    {
        internal Image() { }

        [ComponentModel.Browsable(false)]
        public int Flags { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public Guid[] FrameDimensionsList { get { throw null; } }

        [ComponentModel.DefaultValue(false)]
        [ComponentModel.Browsable(false)]
        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int Height { get { throw null; } }

        public float HorizontalResolution { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public Imaging.ColorPalette Palette { get { throw null; } set { } }

        public SizeF PhysicalDimension { get { throw null; } }

        public Imaging.PixelFormat PixelFormat { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public int[] PropertyIdList { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public Imaging.PropertyItem[] PropertyItems { get { throw null; } }

        public Imaging.ImageFormat RawFormat { get { throw null; } }

        public Size Size { get { throw null; } }

        [ComponentModel.Localizable(false)]
        [ComponentModel.DefaultValue(null)]
        public object? Tag { get { throw null; } set { } }

        public float VerticalResolution { get { throw null; } }

        [ComponentModel.DefaultValue(false)]
        [ComponentModel.Browsable(false)]
        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int Width { get { throw null; } }

        public object Clone() { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~Image() {
        }

        public static Image FromFile(string filename, bool useEmbeddedColorManagement) { throw null; }

        public static Image FromFile(string filename) { throw null; }

        public static Bitmap FromHbitmap(nint hbitmap, nint hpalette) { throw null; }

        public static Bitmap FromHbitmap(nint hbitmap) { throw null; }

        public static Image FromStream(IO.Stream stream, bool useEmbeddedColorManagement, bool validateImageData) { throw null; }

        public static Image FromStream(IO.Stream stream, bool useEmbeddedColorManagement) { throw null; }

        public static Image FromStream(IO.Stream stream) { throw null; }

        public RectangleF GetBounds(ref GraphicsUnit pageUnit) { throw null; }

        public Imaging.EncoderParameters? GetEncoderParameterList(Guid encoder) { throw null; }

        public int GetFrameCount(Imaging.FrameDimension dimension) { throw null; }

        public static int GetPixelFormatSize(Imaging.PixelFormat pixfmt) { throw null; }

        public Imaging.PropertyItem? GetPropertyItem(int propid) { throw null; }

        public Image GetThumbnailImage(int thumbWidth, int thumbHeight, GetThumbnailImageAbort? callback, nint callbackData) { throw null; }

        public static bool IsAlphaPixelFormat(Imaging.PixelFormat pixfmt) { throw null; }

        public static bool IsCanonicalPixelFormat(Imaging.PixelFormat pixfmt) { throw null; }

        public static bool IsExtendedPixelFormat(Imaging.PixelFormat pixfmt) { throw null; }

        public void RemovePropertyItem(int propid) { }

        public void RotateFlip(RotateFlipType rotateFlipType) { }

        public void Save(IO.Stream stream, Imaging.ImageCodecInfo encoder, Imaging.EncoderParameters? encoderParams) { }

        public void Save(IO.Stream stream, Imaging.ImageFormat format) { }

        public void Save(string filename, Imaging.ImageCodecInfo encoder, Imaging.EncoderParameters? encoderParams) { }

        public void Save(string filename, Imaging.ImageFormat format) { }

        public void Save(string filename) { }

        public void SaveAdd(Image image, Imaging.EncoderParameters? encoderParams) { }

        public void SaveAdd(Imaging.EncoderParameters? encoderParams) { }

        public int SelectActiveFrame(Imaging.FrameDimension dimension, int frameIndex) { throw null; }

        public void SetPropertyItem(Imaging.PropertyItem propitem) { }

        void Runtime.Serialization.ISerializable.GetObjectData(Runtime.Serialization.SerializationInfo si, Runtime.Serialization.StreamingContext context) { }

        public delegate bool GetThumbnailImageAbort();
    }

    public sealed partial class ImageAnimator
    {
        internal ImageAnimator() { }

        public static void Animate(Image image, EventHandler onFrameChangedHandler) { }

        public static bool CanAnimate(Image? image) { throw null; }

        public static void StopAnimate(Image image, EventHandler onFrameChangedHandler) { }

        public static void UpdateFrames() { }

        public static void UpdateFrames(Image? image) { }
    }

    public partial class ImageConverter : ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext? context, Type? sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext? context, Type? destinationType) { throw null; }

        public override object? ConvertFrom(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object value) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object? value, Type destinationType) { throw null; }

        [Diagnostics.CodeAnalysis.RequiresUnreferencedCode("The Type of value cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
        public override ComponentModel.PropertyDescriptorCollection GetProperties(ComponentModel.ITypeDescriptorContext? context, object? value, Attribute[]? attributes) { throw null; }

        public override bool GetPropertiesSupported(ComponentModel.ITypeDescriptorContext? context) { throw null; }
    }

    public partial class ImageFormatConverter : ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext? context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext? context, Type? destinationType) { throw null; }

        public override object? ConvertFrom(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object value) { throw null; }

        public override object? ConvertTo(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object? value, Type destinationType) { throw null; }

        public override StandardValuesCollection GetStandardValues(ComponentModel.ITypeDescriptorContext? context) { throw null; }

        public override bool GetStandardValuesSupported(ComponentModel.ITypeDescriptorContext? context) { throw null; }
    }

    public sealed partial class Pen : MarshalByRefObject, ICloneable, IDisposable
    {
        public Pen(Brush brush, float width) { }

        public Pen(Brush brush) { }

        public Pen(Color color, float width) { }

        public Pen(Color color) { }

        public Drawing2D.PenAlignment Alignment { get { throw null; } set { } }

        public Brush Brush { get { throw null; } set { } }

        public Color Color { get { throw null; } set { } }

        public float[] CompoundArray { get { throw null; } set { } }

        public Drawing2D.CustomLineCap CustomEndCap { get { throw null; } set { } }

        public Drawing2D.CustomLineCap CustomStartCap { get { throw null; } set { } }

        public Drawing2D.DashCap DashCap { get { throw null; } set { } }

        public float DashOffset { get { throw null; } set { } }

        public float[] DashPattern { get { throw null; } set { } }

        public Drawing2D.DashStyle DashStyle { get { throw null; } set { } }

        public Drawing2D.LineCap EndCap { get { throw null; } set { } }

        public Drawing2D.LineJoin LineJoin { get { throw null; } set { } }

        public float MiterLimit { get { throw null; } set { } }

        public Drawing2D.PenType PenType { get { throw null; } }

        public Drawing2D.LineCap StartCap { get { throw null; } set { } }

        public Drawing2D.Matrix Transform { get { throw null; } set { } }

        public float Width { get { throw null; } set { } }

        public object Clone() { throw null; }

        public void Dispose() { }

        ~Pen() {
        }

        public void MultiplyTransform(Drawing2D.Matrix matrix, Drawing2D.MatrixOrder order) { }

        public void MultiplyTransform(Drawing2D.Matrix matrix) { }

        public void ResetTransform() { }

        public void RotateTransform(float angle, Drawing2D.MatrixOrder order) { }

        public void RotateTransform(float angle) { }

        public void ScaleTransform(float sx, float sy, Drawing2D.MatrixOrder order) { }

        public void ScaleTransform(float sx, float sy) { }

        public void SetLineCap(Drawing2D.LineCap startCap, Drawing2D.LineCap endCap, Drawing2D.DashCap dashCap) { }

        public void TranslateTransform(float dx, float dy, Drawing2D.MatrixOrder order) { }

        public void TranslateTransform(float dx, float dy) { }
    }

    public static partial class Pens
    {
        public static Pen AliceBlue { get { throw null; } }

        public static Pen AntiqueWhite { get { throw null; } }

        public static Pen Aqua { get { throw null; } }

        public static Pen Aquamarine { get { throw null; } }

        public static Pen Azure { get { throw null; } }

        public static Pen Beige { get { throw null; } }

        public static Pen Bisque { get { throw null; } }

        public static Pen Black { get { throw null; } }

        public static Pen BlanchedAlmond { get { throw null; } }

        public static Pen Blue { get { throw null; } }

        public static Pen BlueViolet { get { throw null; } }

        public static Pen Brown { get { throw null; } }

        public static Pen BurlyWood { get { throw null; } }

        public static Pen CadetBlue { get { throw null; } }

        public static Pen Chartreuse { get { throw null; } }

        public static Pen Chocolate { get { throw null; } }

        public static Pen Coral { get { throw null; } }

        public static Pen CornflowerBlue { get { throw null; } }

        public static Pen Cornsilk { get { throw null; } }

        public static Pen Crimson { get { throw null; } }

        public static Pen Cyan { get { throw null; } }

        public static Pen DarkBlue { get { throw null; } }

        public static Pen DarkCyan { get { throw null; } }

        public static Pen DarkGoldenrod { get { throw null; } }

        public static Pen DarkGray { get { throw null; } }

        public static Pen DarkGreen { get { throw null; } }

        public static Pen DarkKhaki { get { throw null; } }

        public static Pen DarkMagenta { get { throw null; } }

        public static Pen DarkOliveGreen { get { throw null; } }

        public static Pen DarkOrange { get { throw null; } }

        public static Pen DarkOrchid { get { throw null; } }

        public static Pen DarkRed { get { throw null; } }

        public static Pen DarkSalmon { get { throw null; } }

        public static Pen DarkSeaGreen { get { throw null; } }

        public static Pen DarkSlateBlue { get { throw null; } }

        public static Pen DarkSlateGray { get { throw null; } }

        public static Pen DarkTurquoise { get { throw null; } }

        public static Pen DarkViolet { get { throw null; } }

        public static Pen DeepPink { get { throw null; } }

        public static Pen DeepSkyBlue { get { throw null; } }

        public static Pen DimGray { get { throw null; } }

        public static Pen DodgerBlue { get { throw null; } }

        public static Pen Firebrick { get { throw null; } }

        public static Pen FloralWhite { get { throw null; } }

        public static Pen ForestGreen { get { throw null; } }

        public static Pen Fuchsia { get { throw null; } }

        public static Pen Gainsboro { get { throw null; } }

        public static Pen GhostWhite { get { throw null; } }

        public static Pen Gold { get { throw null; } }

        public static Pen Goldenrod { get { throw null; } }

        public static Pen Gray { get { throw null; } }

        public static Pen Green { get { throw null; } }

        public static Pen GreenYellow { get { throw null; } }

        public static Pen Honeydew { get { throw null; } }

        public static Pen HotPink { get { throw null; } }

        public static Pen IndianRed { get { throw null; } }

        public static Pen Indigo { get { throw null; } }

        public static Pen Ivory { get { throw null; } }

        public static Pen Khaki { get { throw null; } }

        public static Pen Lavender { get { throw null; } }

        public static Pen LavenderBlush { get { throw null; } }

        public static Pen LawnGreen { get { throw null; } }

        public static Pen LemonChiffon { get { throw null; } }

        public static Pen LightBlue { get { throw null; } }

        public static Pen LightCoral { get { throw null; } }

        public static Pen LightCyan { get { throw null; } }

        public static Pen LightGoldenrodYellow { get { throw null; } }

        public static Pen LightGray { get { throw null; } }

        public static Pen LightGreen { get { throw null; } }

        public static Pen LightPink { get { throw null; } }

        public static Pen LightSalmon { get { throw null; } }

        public static Pen LightSeaGreen { get { throw null; } }

        public static Pen LightSkyBlue { get { throw null; } }

        public static Pen LightSlateGray { get { throw null; } }

        public static Pen LightSteelBlue { get { throw null; } }

        public static Pen LightYellow { get { throw null; } }

        public static Pen Lime { get { throw null; } }

        public static Pen LimeGreen { get { throw null; } }

        public static Pen Linen { get { throw null; } }

        public static Pen Magenta { get { throw null; } }

        public static Pen Maroon { get { throw null; } }

        public static Pen MediumAquamarine { get { throw null; } }

        public static Pen MediumBlue { get { throw null; } }

        public static Pen MediumOrchid { get { throw null; } }

        public static Pen MediumPurple { get { throw null; } }

        public static Pen MediumSeaGreen { get { throw null; } }

        public static Pen MediumSlateBlue { get { throw null; } }

        public static Pen MediumSpringGreen { get { throw null; } }

        public static Pen MediumTurquoise { get { throw null; } }

        public static Pen MediumVioletRed { get { throw null; } }

        public static Pen MidnightBlue { get { throw null; } }

        public static Pen MintCream { get { throw null; } }

        public static Pen MistyRose { get { throw null; } }

        public static Pen Moccasin { get { throw null; } }

        public static Pen NavajoWhite { get { throw null; } }

        public static Pen Navy { get { throw null; } }

        public static Pen OldLace { get { throw null; } }

        public static Pen Olive { get { throw null; } }

        public static Pen OliveDrab { get { throw null; } }

        public static Pen Orange { get { throw null; } }

        public static Pen OrangeRed { get { throw null; } }

        public static Pen Orchid { get { throw null; } }

        public static Pen PaleGoldenrod { get { throw null; } }

        public static Pen PaleGreen { get { throw null; } }

        public static Pen PaleTurquoise { get { throw null; } }

        public static Pen PaleVioletRed { get { throw null; } }

        public static Pen PapayaWhip { get { throw null; } }

        public static Pen PeachPuff { get { throw null; } }

        public static Pen Peru { get { throw null; } }

        public static Pen Pink { get { throw null; } }

        public static Pen Plum { get { throw null; } }

        public static Pen PowderBlue { get { throw null; } }

        public static Pen Purple { get { throw null; } }

        public static Pen Red { get { throw null; } }

        public static Pen RosyBrown { get { throw null; } }

        public static Pen RoyalBlue { get { throw null; } }

        public static Pen SaddleBrown { get { throw null; } }

        public static Pen Salmon { get { throw null; } }

        public static Pen SandyBrown { get { throw null; } }

        public static Pen SeaGreen { get { throw null; } }

        public static Pen SeaShell { get { throw null; } }

        public static Pen Sienna { get { throw null; } }

        public static Pen Silver { get { throw null; } }

        public static Pen SkyBlue { get { throw null; } }

        public static Pen SlateBlue { get { throw null; } }

        public static Pen SlateGray { get { throw null; } }

        public static Pen Snow { get { throw null; } }

        public static Pen SpringGreen { get { throw null; } }

        public static Pen SteelBlue { get { throw null; } }

        public static Pen Tan { get { throw null; } }

        public static Pen Teal { get { throw null; } }

        public static Pen Thistle { get { throw null; } }

        public static Pen Tomato { get { throw null; } }

        public static Pen Transparent { get { throw null; } }

        public static Pen Turquoise { get { throw null; } }

        public static Pen Violet { get { throw null; } }

        public static Pen Wheat { get { throw null; } }

        public static Pen White { get { throw null; } }

        public static Pen WhiteSmoke { get { throw null; } }

        public static Pen Yellow { get { throw null; } }

        public static Pen YellowGreen { get { throw null; } }
    }

    public sealed partial class Region : MarshalByRefObject, IDisposable
    {
        public Region() { }

        public Region(Drawing2D.GraphicsPath path) { }

        public Region(Drawing2D.RegionData rgnData) { }

        public Region(Rectangle rect) { }

        public Region(RectangleF rect) { }

        public Region Clone() { throw null; }

        public void Complement(Drawing2D.GraphicsPath path) { }

        public void Complement(Rectangle rect) { }

        public void Complement(RectangleF rect) { }

        public void Complement(Region region) { }

        public void Dispose() { }

        public bool Equals(Region region, Graphics g) { throw null; }

        public void Exclude(Drawing2D.GraphicsPath path) { }

        public void Exclude(Rectangle rect) { }

        public void Exclude(RectangleF rect) { }

        public void Exclude(Region region) { }

        ~Region() {
        }

        public static Region FromHrgn(nint hrgn) { throw null; }

        public RectangleF GetBounds(Graphics g) { throw null; }

        public nint GetHrgn(Graphics g) { throw null; }

        public Drawing2D.RegionData? GetRegionData() { throw null; }

        public RectangleF[] GetRegionScans(Drawing2D.Matrix matrix) { throw null; }

        public void Intersect(Drawing2D.GraphicsPath path) { }

        public void Intersect(Rectangle rect) { }

        public void Intersect(RectangleF rect) { }

        public void Intersect(Region region) { }

        public bool IsEmpty(Graphics g) { throw null; }

        public bool IsInfinite(Graphics g) { throw null; }

        public bool IsVisible(Point point, Graphics? g) { throw null; }

        public bool IsVisible(Point point) { throw null; }

        public bool IsVisible(PointF point, Graphics? g) { throw null; }

        public bool IsVisible(PointF point) { throw null; }

        public bool IsVisible(Rectangle rect, Graphics? g) { throw null; }

        public bool IsVisible(Rectangle rect) { throw null; }

        public bool IsVisible(RectangleF rect, Graphics? g) { throw null; }

        public bool IsVisible(RectangleF rect) { throw null; }

        public bool IsVisible(int x, int y, Graphics? g) { throw null; }

        public bool IsVisible(int x, int y, int width, int height, Graphics? g) { throw null; }

        public bool IsVisible(int x, int y, int width, int height) { throw null; }

        public bool IsVisible(float x, float y, Graphics? g) { throw null; }

        public bool IsVisible(float x, float y, float width, float height, Graphics? g) { throw null; }

        public bool IsVisible(float x, float y, float width, float height) { throw null; }

        public bool IsVisible(float x, float y) { throw null; }

        public void MakeEmpty() { }

        public void MakeInfinite() { }

        public void ReleaseHrgn(nint regionHandle) { }

        public void Transform(Drawing2D.Matrix matrix) { }

        public void Translate(int dx, int dy) { }

        public void Translate(float dx, float dy) { }

        public void Union(Drawing2D.GraphicsPath path) { }

        public void Union(Rectangle rect) { }

        public void Union(RectangleF rect) { }

        public void Union(Region region) { }

        public void Xor(Drawing2D.GraphicsPath path) { }

        public void Xor(Rectangle rect) { }

        public void Xor(RectangleF rect) { }

        public void Xor(Region region) { }
    }

    public enum RotateFlipType
    {
        Rotate180FlipXY = 0,
        RotateNoneFlipNone = 0,
        Rotate270FlipXY = 1,
        Rotate90FlipNone = 1,
        Rotate180FlipNone = 2,
        RotateNoneFlipXY = 2,
        Rotate270FlipNone = 3,
        Rotate90FlipXY = 3,
        Rotate180FlipY = 4,
        RotateNoneFlipX = 4,
        Rotate270FlipY = 5,
        Rotate90FlipX = 5,
        Rotate180FlipX = 6,
        RotateNoneFlipY = 6,
        Rotate270FlipX = 7,
        Rotate90FlipY = 7
    }

    public sealed partial class SolidBrush : Brush
    {
        public SolidBrush(Color color) { }

        public Color Color { get { throw null; } set { } }

        public override object Clone() { throw null; }

        protected override void Dispose(bool disposing) { }
    }

    public enum StockIconId
    {
        DocumentNoAssociation = 0,
        DocumentWithAssociation = 1,
        Application = 2,
        Folder = 3,
        FolderOpen = 4,
        Drive525 = 5,
        Drive35 = 6,
        DriveRemovable = 7,
        DriveFixed = 8,
        DriveNet = 9,
        DriveNetDisabled = 10,
        DriveCD = 11,
        DriveRam = 12,
        World = 13,
        Server = 15,
        Printer = 16,
        MyNetwork = 17,
        Find = 22,
        Help = 23,
        Share = 28,
        Link = 29,
        SlowFile = 30,
        Recycler = 31,
        RecyclerFull = 32,
        MediaCDAudio = 40,
        Lock = 47,
        AutoList = 49,
        PrinterNet = 50,
        ServerShare = 51,
        PrinterFax = 52,
        PrinterFaxNet = 53,
        PrinterFile = 54,
        Stack = 55,
        MediaSVCD = 56,
        StuffedFolder = 57,
        DriveUnknown = 58,
        DriveDVD = 59,
        MediaDVD = 60,
        MediaDVDRAM = 61,
        MediaDVDRW = 62,
        MediaDVDR = 63,
        MediaDVDROM = 64,
        MediaCDAudioPlus = 65,
        MediaCDRW = 66,
        MediaCDR = 67,
        MediaCDBurn = 68,
        MediaBlankCD = 69,
        MediaCDROM = 70,
        AudioFiles = 71,
        ImageFiles = 72,
        VideoFiles = 73,
        MixedFiles = 74,
        FolderBack = 75,
        FolderFront = 76,
        Shield = 77,
        Warning = 78,
        Info = 79,
        Error = 80,
        Key = 81,
        Software = 82,
        Rename = 83,
        Delete = 84,
        MediaAudioDVD = 85,
        MediaMovieDVD = 86,
        MediaEnhancedCD = 87,
        MediaEnhancedDVD = 88,
        MediaHDDVD = 89,
        MediaBluRay = 90,
        MediaVCD = 91,
        MediaDVDPlusR = 92,
        MediaDVDPlusRW = 93,
        DesktopPC = 94,
        MobilePC = 95,
        Users = 96,
        MediaSmartMedia = 97,
        MediaCompactFlash = 98,
        DeviceCellPhone = 99,
        DeviceCamera = 100,
        DeviceVideoCamera = 101,
        DeviceAudioPlayer = 102,
        NetworkConnect = 103,
        Internet = 104,
        ZipFile = 105,
        Settings = 106,
        DriveHDDVD = 132,
        DriveBD = 133,
        MediaHDDVDROM = 134,
        MediaHDDVDR = 135,
        MediaHDDVDRAM = 136,
        MediaBDROM = 137,
        MediaBDR = 138,
        MediaBDRE = 139,
        ClusteredDrive = 140
    }

    [Flags]
    public enum StockIconOptions
    {
        Default = 0,
        SmallIcon = 1,
        ShellIconSize = 4,
        LinkOverlay = 32768,
        Selected = 65536
    }

    public enum StringAlignment
    {
        Near = 0,
        Center = 1,
        Far = 2
    }

    public enum StringDigitSubstitute
    {
        User = 0,
        None = 1,
        National = 2,
        Traditional = 3
    }

    public sealed partial class StringFormat : MarshalByRefObject, ICloneable, IDisposable
    {
        public StringFormat() { }

        public StringFormat(StringFormat format) { }

        public StringFormat(StringFormatFlags options, int language) { }

        public StringFormat(StringFormatFlags options) { }

        public StringAlignment Alignment { get { throw null; } set { } }

        public int DigitSubstitutionLanguage { get { throw null; } }

        public StringDigitSubstitute DigitSubstitutionMethod { get { throw null; } }

        public StringFormatFlags FormatFlags { get { throw null; } set { } }

        public static StringFormat GenericDefault { get { throw null; } }

        public static StringFormat GenericTypographic { get { throw null; } }

        public Text.HotkeyPrefix HotkeyPrefix { get { throw null; } set { } }

        public StringAlignment LineAlignment { get { throw null; } set { } }

        public StringTrimming Trimming { get { throw null; } set { } }

        public object Clone() { throw null; }

        public void Dispose() { }

        ~StringFormat() {
        }

        public float[] GetTabStops(out float firstTabOffset) { throw null; }

        public void SetDigitSubstitution(int language, StringDigitSubstitute substitute) { }

        public void SetMeasurableCharacterRanges(CharacterRange[] ranges) { }

        public void SetTabStops(float firstTabOffset, float[] tabStops) { }

        public override string ToString() { throw null; }
    }

    [Flags]
    public enum StringFormatFlags
    {
        DirectionRightToLeft = 1,
        DirectionVertical = 2,
        FitBlackBox = 4,
        DisplayFormatControl = 32,
        NoFontFallback = 1024,
        MeasureTrailingSpaces = 2048,
        NoWrap = 4096,
        LineLimit = 8192,
        NoClip = 16384
    }

    public enum StringTrimming
    {
        None = 0,
        Character = 1,
        Word = 2,
        EllipsisCharacter = 3,
        EllipsisWord = 4,
        EllipsisPath = 5
    }

    public enum StringUnit
    {
        World = 0,
        Display = 1,
        Pixel = 2,
        Point = 3,
        Inch = 4,
        Document = 5,
        Millimeter = 6,
        Em = 32
    }

    public static partial class SystemBrushes
    {
        public static Brush ActiveBorder { get { throw null; } }

        public static Brush ActiveCaption { get { throw null; } }

        public static Brush ActiveCaptionText { get { throw null; } }

        public static Brush AppWorkspace { get { throw null; } }

        public static Brush ButtonFace { get { throw null; } }

        public static Brush ButtonHighlight { get { throw null; } }

        public static Brush ButtonShadow { get { throw null; } }

        public static Brush Control { get { throw null; } }

        public static Brush ControlDark { get { throw null; } }

        public static Brush ControlDarkDark { get { throw null; } }

        public static Brush ControlLight { get { throw null; } }

        public static Brush ControlLightLight { get { throw null; } }

        public static Brush ControlText { get { throw null; } }

        public static Brush Desktop { get { throw null; } }

        public static Brush GradientActiveCaption { get { throw null; } }

        public static Brush GradientInactiveCaption { get { throw null; } }

        public static Brush GrayText { get { throw null; } }

        public static Brush Highlight { get { throw null; } }

        public static Brush HighlightText { get { throw null; } }

        public static Brush HotTrack { get { throw null; } }

        public static Brush InactiveBorder { get { throw null; } }

        public static Brush InactiveCaption { get { throw null; } }

        public static Brush InactiveCaptionText { get { throw null; } }

        public static Brush Info { get { throw null; } }

        public static Brush InfoText { get { throw null; } }

        public static Brush Menu { get { throw null; } }

        public static Brush MenuBar { get { throw null; } }

        public static Brush MenuHighlight { get { throw null; } }

        public static Brush MenuText { get { throw null; } }

        public static Brush ScrollBar { get { throw null; } }

        public static Brush Window { get { throw null; } }

        public static Brush WindowFrame { get { throw null; } }

        public static Brush WindowText { get { throw null; } }

        public static Brush FromSystemColor(Color c) { throw null; }
    }

    public static partial class SystemFonts
    {
        public static Font? CaptionFont { get { throw null; } }

        public static Font DefaultFont { get { throw null; } }

        public static Font DialogFont { get { throw null; } }

        public static Font? IconTitleFont { get { throw null; } }

        public static Font? MenuFont { get { throw null; } }

        public static Font? MessageBoxFont { get { throw null; } }

        public static Font? SmallCaptionFont { get { throw null; } }

        public static Font? StatusFont { get { throw null; } }

        public static Font? GetFontByName(string systemFontName) { throw null; }
    }

    public static partial class SystemIcons
    {
        public static Icon Application { get { throw null; } }

        public static Icon Asterisk { get { throw null; } }

        public static Icon Error { get { throw null; } }

        public static Icon Exclamation { get { throw null; } }

        public static Icon Hand { get { throw null; } }

        public static Icon Information { get { throw null; } }

        public static Icon Question { get { throw null; } }

        public static Icon Shield { get { throw null; } }

        public static Icon Warning { get { throw null; } }

        public static Icon WinLogo { get { throw null; } }

        public static Icon GetStockIcon(StockIconId stockIcon, StockIconOptions options = StockIconOptions.Default) { throw null; }

        public static Icon GetStockIcon(StockIconId stockIcon, int size) { throw null; }
    }

    public static partial class SystemPens
    {
        public static Pen ActiveBorder { get { throw null; } }

        public static Pen ActiveCaption { get { throw null; } }

        public static Pen ActiveCaptionText { get { throw null; } }

        public static Pen AppWorkspace { get { throw null; } }

        public static Pen ButtonFace { get { throw null; } }

        public static Pen ButtonHighlight { get { throw null; } }

        public static Pen ButtonShadow { get { throw null; } }

        public static Pen Control { get { throw null; } }

        public static Pen ControlDark { get { throw null; } }

        public static Pen ControlDarkDark { get { throw null; } }

        public static Pen ControlLight { get { throw null; } }

        public static Pen ControlLightLight { get { throw null; } }

        public static Pen ControlText { get { throw null; } }

        public static Pen Desktop { get { throw null; } }

        public static Pen GradientActiveCaption { get { throw null; } }

        public static Pen GradientInactiveCaption { get { throw null; } }

        public static Pen GrayText { get { throw null; } }

        public static Pen Highlight { get { throw null; } }

        public static Pen HighlightText { get { throw null; } }

        public static Pen HotTrack { get { throw null; } }

        public static Pen InactiveBorder { get { throw null; } }

        public static Pen InactiveCaption { get { throw null; } }

        public static Pen InactiveCaptionText { get { throw null; } }

        public static Pen Info { get { throw null; } }

        public static Pen InfoText { get { throw null; } }

        public static Pen Menu { get { throw null; } }

        public static Pen MenuBar { get { throw null; } }

        public static Pen MenuHighlight { get { throw null; } }

        public static Pen MenuText { get { throw null; } }

        public static Pen ScrollBar { get { throw null; } }

        public static Pen Window { get { throw null; } }

        public static Pen WindowFrame { get { throw null; } }

        public static Pen WindowText { get { throw null; } }

        public static Pen FromSystemColor(Color c) { throw null; }
    }

    public sealed partial class TextureBrush : Brush
    {
        public TextureBrush(Image image, Drawing2D.WrapMode wrapMode, Rectangle dstRect) { }

        public TextureBrush(Image image, Drawing2D.WrapMode wrapMode, RectangleF dstRect) { }

        public TextureBrush(Image image, Drawing2D.WrapMode wrapMode) { }

        public TextureBrush(Image image, Rectangle dstRect, Imaging.ImageAttributes? imageAttr) { }

        public TextureBrush(Image image, Rectangle dstRect) { }

        public TextureBrush(Image image, RectangleF dstRect, Imaging.ImageAttributes? imageAttr) { }

        public TextureBrush(Image image, RectangleF dstRect) { }

        public TextureBrush(Image bitmap) { }

        public Image Image { get { throw null; } }

        public Drawing2D.Matrix Transform { get { throw null; } set { } }

        public Drawing2D.WrapMode WrapMode { get { throw null; } set { } }

        public override object Clone() { throw null; }

        public void MultiplyTransform(Drawing2D.Matrix matrix, Drawing2D.MatrixOrder order) { }

        public void MultiplyTransform(Drawing2D.Matrix matrix) { }

        public void ResetTransform() { }

        public void RotateTransform(float angle, Drawing2D.MatrixOrder order) { }

        public void RotateTransform(float angle) { }

        public void ScaleTransform(float sx, float sy, Drawing2D.MatrixOrder order) { }

        public void ScaleTransform(float sx, float sy) { }

        public void TranslateTransform(float dx, float dy, Drawing2D.MatrixOrder order) { }

        public void TranslateTransform(float dx, float dy) { }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public partial class ToolboxBitmapAttribute : Attribute
    {
        public static readonly ToolboxBitmapAttribute Default;
        public ToolboxBitmapAttribute(string imageFile) { }

        public ToolboxBitmapAttribute(Type t, string name) { }

        public ToolboxBitmapAttribute(Type t) { }

        public override bool Equals(object? value) { throw null; }

        public override int GetHashCode() { throw null; }

        public Image? GetImage(object? component, bool large) { throw null; }

        public Image? GetImage(object? component) { throw null; }

        public Image? GetImage(Type type, bool large) { throw null; }

        public Image? GetImage(Type type, string? imgName, bool large) { throw null; }

        public Image? GetImage(Type type) { throw null; }

        public static Image? GetImageFromResource(Type t, string? imageName, bool large) { throw null; }
    }
}

namespace System.Drawing.Design
{
    public sealed partial class CategoryNameCollection : Collections.ReadOnlyCollectionBase
    {
        public CategoryNameCollection(CategoryNameCollection value) { }

        public CategoryNameCollection(string[] value) { }

        public string this[int index] { get { throw null; } }

        public bool Contains(string value) { throw null; }

        public void CopyTo(string[] array, int index) { }

        public int IndexOf(string value) { throw null; }
    }
}

namespace System.Drawing.Drawing2D
{
    public sealed partial class AdjustableArrowCap : CustomLineCap
    {
        public AdjustableArrowCap(float width, float height, bool isFilled) : base(default, default) { }

        public AdjustableArrowCap(float width, float height) : base(default, default) { }

        public bool Filled { get { throw null; } set { } }

        public float Height { get { throw null; } set { } }

        public float MiddleInset { get { throw null; } set { } }

        public float Width { get { throw null; } set { } }
    }

    public sealed partial class Blend
    {
        public Blend() { }

        public Blend(int count) { }

        public float[] Factors { get { throw null; } set { } }

        public float[] Positions { get { throw null; } set { } }
    }

    public sealed partial class ColorBlend
    {
        public ColorBlend() { }

        public ColorBlend(int count) { }

        public Color[] Colors { get { throw null; } set { } }

        public float[] Positions { get { throw null; } set { } }
    }

    public enum CombineMode
    {
        Replace = 0,
        Intersect = 1,
        Union = 2,
        Xor = 3,
        Exclude = 4,
        Complement = 5
    }

    public enum CompositingMode
    {
        SourceOver = 0,
        SourceCopy = 1
    }

    public enum CompositingQuality
    {
        Invalid = -1,
        Default = 0,
        HighSpeed = 1,
        HighQuality = 2,
        GammaCorrected = 3,
        AssumeLinear = 4
    }

    public enum CoordinateSpace
    {
        World = 0,
        Page = 1,
        Device = 2
    }

    public partial class CustomLineCap : MarshalByRefObject, ICloneable, IDisposable
    {
        public CustomLineCap(GraphicsPath? fillPath, GraphicsPath? strokePath, LineCap baseCap, float baseInset) { }

        public CustomLineCap(GraphicsPath? fillPath, GraphicsPath? strokePath, LineCap baseCap) { }

        public CustomLineCap(GraphicsPath? fillPath, GraphicsPath? strokePath) { }

        public LineCap BaseCap { get { throw null; } set { } }

        public float BaseInset { get { throw null; } set { } }

        public LineJoin StrokeJoin { get { throw null; } set { } }

        public float WidthScale { get { throw null; } set { } }

        public object Clone() { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~CustomLineCap() {
        }

        public void GetStrokeCaps(out LineCap startCap, out LineCap endCap) { throw null; }

        public void SetStrokeCaps(LineCap startCap, LineCap endCap) { }
    }

    public enum DashCap
    {
        Flat = 0,
        Round = 2,
        Triangle = 3
    }

    public enum DashStyle
    {
        Solid = 0,
        Dash = 1,
        Dot = 2,
        DashDot = 3,
        DashDotDot = 4,
        Custom = 5
    }

    public enum FillMode
    {
        Alternate = 0,
        Winding = 1
    }

    public enum FlushIntention
    {
        Flush = 0,
        Sync = 1
    }

    public sealed partial class GraphicsContainer : MarshalByRefObject
    {
        internal GraphicsContainer() { }
    }

    public sealed partial class GraphicsPath : MarshalByRefObject, ICloneable, IDisposable
    {
        public GraphicsPath() { }

        public GraphicsPath(FillMode fillMode) { }

        public GraphicsPath(Point[] pts, byte[] types, FillMode fillMode) { }

        public GraphicsPath(Point[] pts, byte[] types) { }

        public GraphicsPath(PointF[] pts, byte[] types, FillMode fillMode) { }

        public GraphicsPath(PointF[] pts, byte[] types) { }

        public FillMode FillMode { get { throw null; } set { } }

        public PathData PathData { get { throw null; } }

        public PointF[] PathPoints { get { throw null; } }

        public byte[] PathTypes { get { throw null; } }

        public int PointCount { get { throw null; } }

        public void AddArc(Rectangle rect, float startAngle, float sweepAngle) { }

        public void AddArc(RectangleF rect, float startAngle, float sweepAngle) { }

        public void AddArc(int x, int y, int width, int height, float startAngle, float sweepAngle) { }

        public void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle) { }

        public void AddBezier(Point pt1, Point pt2, Point pt3, Point pt4) { }

        public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4) { }

        public void AddBezier(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4) { }

        public void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) { }

        public void AddBeziers(params Point[] points) { }

        public void AddBeziers(PointF[] points) { }

        public void AddClosedCurve(Point[] points, float tension) { }

        public void AddClosedCurve(Point[] points) { }

        public void AddClosedCurve(PointF[] points, float tension) { }

        public void AddClosedCurve(PointF[] points) { }

        public void AddCurve(Point[] points, int offset, int numberOfSegments, float tension) { }

        public void AddCurve(Point[] points, float tension) { }

        public void AddCurve(Point[] points) { }

        public void AddCurve(PointF[] points, int offset, int numberOfSegments, float tension) { }

        public void AddCurve(PointF[] points, float tension) { }

        public void AddCurve(PointF[] points) { }

        public void AddEllipse(Rectangle rect) { }

        public void AddEllipse(RectangleF rect) { }

        public void AddEllipse(int x, int y, int width, int height) { }

        public void AddEllipse(float x, float y, float width, float height) { }

        public void AddLine(Point pt1, Point pt2) { }

        public void AddLine(PointF pt1, PointF pt2) { }

        public void AddLine(int x1, int y1, int x2, int y2) { }

        public void AddLine(float x1, float y1, float x2, float y2) { }

        public void AddLines(Point[] points) { }

        public void AddLines(PointF[] points) { }

        public void AddPath(GraphicsPath addingPath, bool connect) { }

        public void AddPie(Rectangle rect, float startAngle, float sweepAngle) { }

        public void AddPie(int x, int y, int width, int height, float startAngle, float sweepAngle) { }

        public void AddPie(float x, float y, float width, float height, float startAngle, float sweepAngle) { }

        public void AddPolygon(Point[] points) { }

        public void AddPolygon(PointF[] points) { }

        public void AddRectangle(Rectangle rect) { }

        public void AddRectangle(RectangleF rect) { }

        public void AddRectangles(Rectangle[] rects) { }

        public void AddRectangles(RectangleF[] rects) { }

        public void AddString(string s, FontFamily family, int style, float emSize, Point origin, StringFormat? format) { }

        public void AddString(string s, FontFamily family, int style, float emSize, PointF origin, StringFormat? format) { }

        public void AddString(string s, FontFamily family, int style, float emSize, Rectangle layoutRect, StringFormat? format) { }

        public void AddString(string s, FontFamily family, int style, float emSize, RectangleF layoutRect, StringFormat? format) { }

        public void ClearMarkers() { }

        public object Clone() { throw null; }

        public void CloseAllFigures() { }

        public void CloseFigure() { }

        public void Dispose() { }

        ~GraphicsPath() {
        }

        public void Flatten() { }

        public void Flatten(Matrix? matrix, float flatness) { }

        public void Flatten(Matrix? matrix) { }

        public RectangleF GetBounds() { throw null; }

        public RectangleF GetBounds(Matrix? matrix, Pen? pen) { throw null; }

        public RectangleF GetBounds(Matrix? matrix) { throw null; }

        public PointF GetLastPoint() { throw null; }

        public bool IsOutlineVisible(Point pt, Pen pen, Graphics? graphics) { throw null; }

        public bool IsOutlineVisible(Point point, Pen pen) { throw null; }

        public bool IsOutlineVisible(PointF pt, Pen pen, Graphics? graphics) { throw null; }

        public bool IsOutlineVisible(PointF point, Pen pen) { throw null; }

        public bool IsOutlineVisible(int x, int y, Pen pen, Graphics? graphics) { throw null; }

        public bool IsOutlineVisible(int x, int y, Pen pen) { throw null; }

        public bool IsOutlineVisible(float x, float y, Pen pen, Graphics? graphics) { throw null; }

        public bool IsOutlineVisible(float x, float y, Pen pen) { throw null; }

        public bool IsVisible(Point pt, Graphics? graphics) { throw null; }

        public bool IsVisible(Point point) { throw null; }

        public bool IsVisible(PointF pt, Graphics? graphics) { throw null; }

        public bool IsVisible(PointF point) { throw null; }

        public bool IsVisible(int x, int y, Graphics? graphics) { throw null; }

        public bool IsVisible(int x, int y) { throw null; }

        public bool IsVisible(float x, float y, Graphics? graphics) { throw null; }

        public bool IsVisible(float x, float y) { throw null; }

        public void Reset() { }

        public void Reverse() { }

        public void SetMarkers() { }

        public void StartFigure() { }

        public void Transform(Matrix matrix) { }

        public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix? matrix, WarpMode warpMode, float flatness) { }

        public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix? matrix, WarpMode warpMode) { }

        public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix? matrix) { }

        public void Warp(PointF[] destPoints, RectangleF srcRect) { }

        public void Widen(Pen pen, Matrix? matrix, float flatness) { }

        public void Widen(Pen pen, Matrix? matrix) { }

        public void Widen(Pen pen) { }
    }

    public sealed partial class GraphicsPathIterator : MarshalByRefObject, IDisposable
    {
        public GraphicsPathIterator(GraphicsPath? path) { }

        public int Count { get { throw null; } }

        public int SubpathCount { get { throw null; } }

        public int CopyData(ref PointF[] points, ref byte[] types, int startIndex, int endIndex) { throw null; }

        public void Dispose() { }

        public int Enumerate(ref PointF[] points, ref byte[] types) { throw null; }

        ~GraphicsPathIterator() {
        }

        public bool HasCurve() { throw null; }

        public int NextMarker(GraphicsPath path) { throw null; }

        public int NextMarker(out int startIndex, out int endIndex) { throw null; }

        public int NextPathType(out byte pathType, out int startIndex, out int endIndex) { throw null; }

        public int NextSubpath(GraphicsPath path, out bool isClosed) { throw null; }

        public int NextSubpath(out int startIndex, out int endIndex, out bool isClosed) { throw null; }

        public void Rewind() { }
    }

    public sealed partial class GraphicsState : MarshalByRefObject
    {
        internal GraphicsState() { }
    }

    public sealed partial class HatchBrush : Brush
    {
        public HatchBrush(HatchStyle hatchstyle, Color foreColor, Color backColor) { }

        public HatchBrush(HatchStyle hatchstyle, Color foreColor) { }

        public Color BackgroundColor { get { throw null; } }

        public Color ForegroundColor { get { throw null; } }

        public HatchStyle HatchStyle { get { throw null; } }

        public override object Clone() { throw null; }
    }

    public enum HatchStyle
    {
        Horizontal = 0,
        Min = 0,
        Vertical = 1,
        ForwardDiagonal = 2,
        BackwardDiagonal = 3,
        Cross = 4,
        LargeGrid = 4,
        Max = 4,
        DiagonalCross = 5,
        Percent05 = 6,
        Percent10 = 7,
        Percent20 = 8,
        Percent25 = 9,
        Percent30 = 10,
        Percent40 = 11,
        Percent50 = 12,
        Percent60 = 13,
        Percent70 = 14,
        Percent75 = 15,
        Percent80 = 16,
        Percent90 = 17,
        LightDownwardDiagonal = 18,
        LightUpwardDiagonal = 19,
        DarkDownwardDiagonal = 20,
        DarkUpwardDiagonal = 21,
        WideDownwardDiagonal = 22,
        WideUpwardDiagonal = 23,
        LightVertical = 24,
        LightHorizontal = 25,
        NarrowVertical = 26,
        NarrowHorizontal = 27,
        DarkVertical = 28,
        DarkHorizontal = 29,
        DashedDownwardDiagonal = 30,
        DashedUpwardDiagonal = 31,
        DashedHorizontal = 32,
        DashedVertical = 33,
        SmallConfetti = 34,
        LargeConfetti = 35,
        ZigZag = 36,
        Wave = 37,
        DiagonalBrick = 38,
        HorizontalBrick = 39,
        Weave = 40,
        Plaid = 41,
        Divot = 42,
        DottedGrid = 43,
        DottedDiamond = 44,
        Shingle = 45,
        Trellis = 46,
        Sphere = 47,
        SmallGrid = 48,
        SmallCheckerBoard = 49,
        LargeCheckerBoard = 50,
        OutlinedDiamond = 51,
        SolidDiamond = 52
    }

    public enum InterpolationMode
    {
        Invalid = -1,
        Default = 0,
        Low = 1,
        High = 2,
        Bilinear = 3,
        Bicubic = 4,
        NearestNeighbor = 5,
        HighQualityBilinear = 6,
        HighQualityBicubic = 7
    }

    public sealed partial class LinearGradientBrush : Brush
    {
        public LinearGradientBrush(Point point1, Point point2, Color color1, Color color2) { }

        public LinearGradientBrush(PointF point1, PointF point2, Color color1, Color color2) { }

        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, LinearGradientMode linearGradientMode) { }

        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle, bool isAngleScaleable) { }

        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle) { }

        public LinearGradientBrush(RectangleF rect, Color color1, Color color2, LinearGradientMode linearGradientMode) { }

        public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle, bool isAngleScaleable) { }

        public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle) { }

        public Blend? Blend { get { throw null; } set { } }

        public bool GammaCorrection { get { throw null; } set { } }

        public ColorBlend InterpolationColors { get { throw null; } set { } }

        public Color[] LinearColors { get { throw null; } set { } }

        public RectangleF Rectangle { get { throw null; } }

        public Matrix Transform { get { throw null; } set { } }

        public WrapMode WrapMode { get { throw null; } set { } }

        public override object Clone() { throw null; }

        public void MultiplyTransform(Matrix matrix, MatrixOrder order) { }

        public void MultiplyTransform(Matrix matrix) { }

        public void ResetTransform() { }

        public void RotateTransform(float angle, MatrixOrder order) { }

        public void RotateTransform(float angle) { }

        public void ScaleTransform(float sx, float sy, MatrixOrder order) { }

        public void ScaleTransform(float sx, float sy) { }

        public void SetBlendTriangularShape(float focus, float scale) { }

        public void SetBlendTriangularShape(float focus) { }

        public void SetSigmaBellShape(float focus, float scale) { }

        public void SetSigmaBellShape(float focus) { }

        public void TranslateTransform(float dx, float dy, MatrixOrder order) { }

        public void TranslateTransform(float dx, float dy) { }
    }

    public enum LinearGradientMode
    {
        Horizontal = 0,
        Vertical = 1,
        ForwardDiagonal = 2,
        BackwardDiagonal = 3
    }

    public enum LineCap
    {
        Flat = 0,
        Square = 1,
        Round = 2,
        Triangle = 3,
        NoAnchor = 16,
        SquareAnchor = 17,
        RoundAnchor = 18,
        DiamondAnchor = 19,
        ArrowAnchor = 20,
        AnchorMask = 240,
        Custom = 255
    }

    public enum LineJoin
    {
        Miter = 0,
        Bevel = 1,
        Round = 2,
        MiterClipped = 3
    }

    public sealed partial class Matrix : MarshalByRefObject, IDisposable
    {
        public Matrix() { }

        public Matrix(Rectangle rect, Point[] plgpts) { }

        public Matrix(RectangleF rect, PointF[] plgpts) { }

        public Matrix(Numerics.Matrix3x2 matrix) { }

        public Matrix(float m11, float m12, float m21, float m22, float dx, float dy) { }

        public float[] Elements { get { throw null; } }

        public bool IsIdentity { get { throw null; } }

        public bool IsInvertible { get { throw null; } }

        public Numerics.Matrix3x2 MatrixElements { get { throw null; } set { } }

        public float OffsetX { get { throw null; } }

        public float OffsetY { get { throw null; } }

        public Matrix Clone() { throw null; }

        public void Dispose() { }

        public override bool Equals(object? obj) { throw null; }

        ~Matrix() {
        }

        public override int GetHashCode() { throw null; }

        public void Invert() { }

        public void Multiply(Matrix matrix, MatrixOrder order) { }

        public void Multiply(Matrix matrix) { }

        public void Reset() { }

        public void Rotate(float angle, MatrixOrder order) { }

        public void Rotate(float angle) { }

        public void RotateAt(float angle, PointF point, MatrixOrder order) { }

        public void RotateAt(float angle, PointF point) { }

        public void Scale(float scaleX, float scaleY, MatrixOrder order) { }

        public void Scale(float scaleX, float scaleY) { }

        public void Shear(float shearX, float shearY, MatrixOrder order) { }

        public void Shear(float shearX, float shearY) { }

        public void TransformPoints(Point[] pts) { }

        public void TransformPoints(PointF[] pts) { }

        public void TransformVectors(Point[] pts) { }

        public void TransformVectors(PointF[] pts) { }

        public void Translate(float offsetX, float offsetY, MatrixOrder order) { }

        public void Translate(float offsetX, float offsetY) { }

        public void VectorTransformPoints(Point[] pts) { }
    }

    public enum MatrixOrder
    {
        Prepend = 0,
        Append = 1
    }

    public sealed partial class PathData
    {
        public PointF[]? Points { get { throw null; } set { } }

        public byte[]? Types { get { throw null; } set { } }
    }

    public sealed partial class PathGradientBrush : Brush
    {
        public PathGradientBrush(GraphicsPath path) { }

        public PathGradientBrush(Point[] points, WrapMode wrapMode) { }

        public PathGradientBrush(Point[] points) { }

        public PathGradientBrush(PointF[] points, WrapMode wrapMode) { }

        public PathGradientBrush(PointF[] points) { }

        public Blend Blend { get { throw null; } set { } }

        public Color CenterColor { get { throw null; } set { } }

        public PointF CenterPoint { get { throw null; } set { } }

        public PointF FocusScales { get { throw null; } set { } }

        public ColorBlend InterpolationColors { get { throw null; } set { } }

        public RectangleF Rectangle { get { throw null; } }

        public Color[] SurroundColors { get { throw null; } set { } }

        public Matrix Transform { get { throw null; } set { } }

        public WrapMode WrapMode { get { throw null; } set { } }

        public override object Clone() { throw null; }

        public void MultiplyTransform(Matrix matrix, MatrixOrder order) { }

        public void MultiplyTransform(Matrix matrix) { }

        public void ResetTransform() { }

        public void RotateTransform(float angle, MatrixOrder order) { }

        public void RotateTransform(float angle) { }

        public void ScaleTransform(float sx, float sy, MatrixOrder order) { }

        public void ScaleTransform(float sx, float sy) { }

        public void SetBlendTriangularShape(float focus, float scale) { }

        public void SetBlendTriangularShape(float focus) { }

        public void SetSigmaBellShape(float focus, float scale) { }

        public void SetSigmaBellShape(float focus) { }

        public void TranslateTransform(float dx, float dy, MatrixOrder order) { }

        public void TranslateTransform(float dx, float dy) { }
    }

    public enum PathPointType
    {
        Start = 0,
        Line = 1,
        Bezier = 3,
        Bezier3 = 3,
        PathTypeMask = 7,
        DashMode = 16,
        PathMarker = 32,
        CloseSubpath = 128
    }

    public enum PenAlignment
    {
        Center = 0,
        Inset = 1,
        Outset = 2,
        Left = 3,
        Right = 4
    }

    public enum PenType
    {
        SolidColor = 0,
        HatchFill = 1,
        TextureFill = 2,
        PathGradient = 3,
        LinearGradient = 4
    }

    public enum PixelOffsetMode
    {
        Invalid = -1,
        Default = 0,
        HighSpeed = 1,
        HighQuality = 2,
        None = 3,
        Half = 4
    }

    public enum QualityMode
    {
        Invalid = -1,
        Default = 0,
        Low = 1,
        High = 2
    }

    public sealed partial class RegionData
    {
        internal RegionData() { }

        public byte[] Data { get { throw null; } set { } }
    }

    public enum SmoothingMode
    {
        Invalid = -1,
        Default = 0,
        HighSpeed = 1,
        HighQuality = 2,
        None = 3,
        AntiAlias = 4
    }

    public enum WarpMode
    {
        Perspective = 0,
        Bilinear = 1
    }

    public enum WrapMode
    {
        Tile = 0,
        TileFlipX = 1,
        TileFlipY = 2,
        TileFlipXY = 3,
        Clamp = 4
    }
}

namespace System.Drawing.Imaging
{
    public sealed partial class BitmapData
    {
        public int Height { get { throw null; } set { } }

        public PixelFormat PixelFormat { get { throw null; } set { } }

        public int Reserved { get { throw null; } set { } }

        public nint Scan0 { get { throw null; } set { } }

        public int Stride { get { throw null; } set { } }

        public int Width { get { throw null; } set { } }
    }

    public sealed partial class CachedBitmap : IDisposable
    {
        public CachedBitmap(Bitmap bitmap, Graphics graphics) { }

        public void Dispose() { }

        ~CachedBitmap() {
        }
    }

    public enum ColorAdjustType
    {
        Default = 0,
        Bitmap = 1,
        Brush = 2,
        Pen = 3,
        Text = 4,
        Count = 5,
        Any = 6
    }

    public enum ColorChannelFlag
    {
        ColorChannelC = 0,
        ColorChannelM = 1,
        ColorChannelY = 2,
        ColorChannelK = 3,
        ColorChannelLast = 4
    }

    public sealed partial class ColorMap
    {
        public Color NewColor { get { throw null; } set { } }

        public Color OldColor { get { throw null; } set { } }
    }

    public enum ColorMapType
    {
        Default = 0,
        Brush = 1
    }

    public sealed partial class ColorMatrix
    {
        public ColorMatrix() { }

        [CLSCompliant(false)]
        public ColorMatrix(float[][] newColorMatrix) { }

        public float this[int row, int column] { get { throw null; } set { } }

        public float Matrix00 { get { throw null; } set { } }

        public float Matrix01 { get { throw null; } set { } }

        public float Matrix02 { get { throw null; } set { } }

        public float Matrix03 { get { throw null; } set { } }

        public float Matrix04 { get { throw null; } set { } }

        public float Matrix10 { get { throw null; } set { } }

        public float Matrix11 { get { throw null; } set { } }

        public float Matrix12 { get { throw null; } set { } }

        public float Matrix13 { get { throw null; } set { } }

        public float Matrix14 { get { throw null; } set { } }

        public float Matrix20 { get { throw null; } set { } }

        public float Matrix21 { get { throw null; } set { } }

        public float Matrix22 { get { throw null; } set { } }

        public float Matrix23 { get { throw null; } set { } }

        public float Matrix24 { get { throw null; } set { } }

        public float Matrix30 { get { throw null; } set { } }

        public float Matrix31 { get { throw null; } set { } }

        public float Matrix32 { get { throw null; } set { } }

        public float Matrix33 { get { throw null; } set { } }

        public float Matrix34 { get { throw null; } set { } }

        public float Matrix40 { get { throw null; } set { } }

        public float Matrix41 { get { throw null; } set { } }

        public float Matrix42 { get { throw null; } set { } }

        public float Matrix43 { get { throw null; } set { } }

        public float Matrix44 { get { throw null; } set { } }
    }

    public enum ColorMatrixFlag
    {
        Default = 0,
        SkipGrays = 1,
        AltGrays = 2
    }

    public enum ColorMode
    {
        Argb32Mode = 0,
        Argb64Mode = 1
    }

    public sealed partial class ColorPalette
    {
        internal ColorPalette() { }

        public Color[] Entries { get { throw null; } }

        public int Flags { get { throw null; } }
    }

    public enum EmfPlusRecordType
    {
        EmfHeader = 1,
        EmfMin = 1,
        EmfPolyBezier = 2,
        EmfPolygon = 3,
        EmfPolyline = 4,
        EmfPolyBezierTo = 5,
        EmfPolyLineTo = 6,
        EmfPolyPolyline = 7,
        EmfPolyPolygon = 8,
        EmfSetWindowExtEx = 9,
        EmfSetWindowOrgEx = 10,
        EmfSetViewportExtEx = 11,
        EmfSetViewportOrgEx = 12,
        EmfSetBrushOrgEx = 13,
        EmfEof = 14,
        EmfSetPixelV = 15,
        EmfSetMapperFlags = 16,
        EmfSetMapMode = 17,
        EmfSetBkMode = 18,
        EmfSetPolyFillMode = 19,
        EmfSetROP2 = 20,
        EmfSetStretchBltMode = 21,
        EmfSetTextAlign = 22,
        EmfSetColorAdjustment = 23,
        EmfSetTextColor = 24,
        EmfSetBkColor = 25,
        EmfOffsetClipRgn = 26,
        EmfMoveToEx = 27,
        EmfSetMetaRgn = 28,
        EmfExcludeClipRect = 29,
        EmfIntersectClipRect = 30,
        EmfScaleViewportExtEx = 31,
        EmfScaleWindowExtEx = 32,
        EmfSaveDC = 33,
        EmfRestoreDC = 34,
        EmfSetWorldTransform = 35,
        EmfModifyWorldTransform = 36,
        EmfSelectObject = 37,
        EmfCreatePen = 38,
        EmfCreateBrushIndirect = 39,
        EmfDeleteObject = 40,
        EmfAngleArc = 41,
        EmfEllipse = 42,
        EmfRectangle = 43,
        EmfRoundRect = 44,
        EmfRoundArc = 45,
        EmfChord = 46,
        EmfPie = 47,
        EmfSelectPalette = 48,
        EmfCreatePalette = 49,
        EmfSetPaletteEntries = 50,
        EmfResizePalette = 51,
        EmfRealizePalette = 52,
        EmfExtFloodFill = 53,
        EmfLineTo = 54,
        EmfArcTo = 55,
        EmfPolyDraw = 56,
        EmfSetArcDirection = 57,
        EmfSetMiterLimit = 58,
        EmfBeginPath = 59,
        EmfEndPath = 60,
        EmfCloseFigure = 61,
        EmfFillPath = 62,
        EmfStrokeAndFillPath = 63,
        EmfStrokePath = 64,
        EmfFlattenPath = 65,
        EmfWidenPath = 66,
        EmfSelectClipPath = 67,
        EmfAbortPath = 68,
        EmfReserved069 = 69,
        EmfGdiComment = 70,
        EmfFillRgn = 71,
        EmfFrameRgn = 72,
        EmfInvertRgn = 73,
        EmfPaintRgn = 74,
        EmfExtSelectClipRgn = 75,
        EmfBitBlt = 76,
        EmfStretchBlt = 77,
        EmfMaskBlt = 78,
        EmfPlgBlt = 79,
        EmfSetDIBitsToDevice = 80,
        EmfStretchDIBits = 81,
        EmfExtCreateFontIndirect = 82,
        EmfExtTextOutA = 83,
        EmfExtTextOutW = 84,
        EmfPolyBezier16 = 85,
        EmfPolygon16 = 86,
        EmfPolyline16 = 87,
        EmfPolyBezierTo16 = 88,
        EmfPolylineTo16 = 89,
        EmfPolyPolyline16 = 90,
        EmfPolyPolygon16 = 91,
        EmfPolyDraw16 = 92,
        EmfCreateMonoBrush = 93,
        EmfCreateDibPatternBrushPt = 94,
        EmfExtCreatePen = 95,
        EmfPolyTextOutA = 96,
        EmfPolyTextOutW = 97,
        EmfSetIcmMode = 98,
        EmfCreateColorSpace = 99,
        EmfSetColorSpace = 100,
        EmfDeleteColorSpace = 101,
        EmfGlsRecord = 102,
        EmfGlsBoundedRecord = 103,
        EmfPixelFormat = 104,
        EmfDrawEscape = 105,
        EmfExtEscape = 106,
        EmfStartDoc = 107,
        EmfSmallTextOut = 108,
        EmfForceUfiMapping = 109,
        EmfNamedEscpae = 110,
        EmfColorCorrectPalette = 111,
        EmfSetIcmProfileA = 112,
        EmfSetIcmProfileW = 113,
        EmfAlphaBlend = 114,
        EmfSetLayout = 115,
        EmfTransparentBlt = 116,
        EmfReserved117 = 117,
        EmfGradientFill = 118,
        EmfSetLinkedUfis = 119,
        EmfSetTextJustification = 120,
        EmfColorMatchToTargetW = 121,
        EmfCreateColorSpaceW = 122,
        EmfMax = 122,
        EmfPlusRecordBase = 16384,
        Invalid = 16384,
        Header = 16385,
        Min = 16385,
        EndOfFile = 16386,
        Comment = 16387,
        GetDC = 16388,
        MultiFormatStart = 16389,
        MultiFormatSection = 16390,
        MultiFormatEnd = 16391,
        Object = 16392,
        Clear = 16393,
        FillRects = 16394,
        DrawRects = 16395,
        FillPolygon = 16396,
        DrawLines = 16397,
        FillEllipse = 16398,
        DrawEllipse = 16399,
        FillPie = 16400,
        DrawPie = 16401,
        DrawArc = 16402,
        FillRegion = 16403,
        FillPath = 16404,
        DrawPath = 16405,
        FillClosedCurve = 16406,
        DrawClosedCurve = 16407,
        DrawCurve = 16408,
        DrawBeziers = 16409,
        DrawImage = 16410,
        DrawImagePoints = 16411,
        DrawString = 16412,
        SetRenderingOrigin = 16413,
        SetAntiAliasMode = 16414,
        SetTextRenderingHint = 16415,
        SetTextContrast = 16416,
        SetInterpolationMode = 16417,
        SetPixelOffsetMode = 16418,
        SetCompositingMode = 16419,
        SetCompositingQuality = 16420,
        Save = 16421,
        Restore = 16422,
        BeginContainer = 16423,
        BeginContainerNoParams = 16424,
        EndContainer = 16425,
        SetWorldTransform = 16426,
        ResetWorldTransform = 16427,
        MultiplyWorldTransform = 16428,
        TranslateWorldTransform = 16429,
        ScaleWorldTransform = 16430,
        RotateWorldTransform = 16431,
        SetPageTransform = 16432,
        ResetClip = 16433,
        SetClipRect = 16434,
        SetClipPath = 16435,
        SetClipRegion = 16436,
        OffsetClip = 16437,
        DrawDriverString = 16438,
        Max = 16438,
        Total = 16439,
        WmfRecordBase = 65536,
        WmfSaveDC = 65566,
        WmfRealizePalette = 65589,
        WmfSetPalEntries = 65591,
        WmfCreatePalette = 65783,
        WmfSetBkMode = 65794,
        WmfSetMapMode = 65795,
        WmfSetROP2 = 65796,
        WmfSetRelAbs = 65797,
        WmfSetPolyFillMode = 65798,
        WmfSetStretchBltMode = 65799,
        WmfSetTextCharExtra = 65800,
        WmfRestoreDC = 65831,
        WmfInvertRegion = 65834,
        WmfPaintRegion = 65835,
        WmfSelectClipRegion = 65836,
        WmfSelectObject = 65837,
        WmfSetTextAlign = 65838,
        WmfResizePalette = 65849,
        WmfDibCreatePatternBrush = 65858,
        WmfSetLayout = 65865,
        WmfDeleteObject = 66032,
        WmfCreatePatternBrush = 66041,
        WmfSetBkColor = 66049,
        WmfSetTextColor = 66057,
        WmfSetTextJustification = 66058,
        WmfSetWindowOrg = 66059,
        WmfSetWindowExt = 66060,
        WmfSetViewportOrg = 66061,
        WmfSetViewportExt = 66062,
        WmfOffsetWindowOrg = 66063,
        WmfOffsetViewportOrg = 66065,
        WmfLineTo = 66067,
        WmfMoveTo = 66068,
        WmfOffsetCilpRgn = 66080,
        WmfFillRegion = 66088,
        WmfSetMapperFlags = 66097,
        WmfSelectPalette = 66100,
        WmfCreatePenIndirect = 66298,
        WmfCreateFontIndirect = 66299,
        WmfCreateBrushIndirect = 66300,
        WmfPolygon = 66340,
        WmfPolyline = 66341,
        WmfScaleWindowExt = 66576,
        WmfScaleViewportExt = 66578,
        WmfExcludeClipRect = 66581,
        WmfIntersectClipRect = 66582,
        WmfEllipse = 66584,
        WmfFloodFill = 66585,
        WmfRectangle = 66587,
        WmfSetPixel = 66591,
        WmfFrameRegion = 66601,
        WmfAnimatePalette = 66614,
        WmfTextOut = 66849,
        WmfPolyPolygon = 66872,
        WmfExtFloodFill = 66888,
        WmfRoundRect = 67100,
        WmfPatBlt = 67101,
        WmfEscape = 67110,
        WmfCreateRegion = 67327,
        WmfArc = 67607,
        WmfPie = 67610,
        WmfChord = 67632,
        WmfBitBlt = 67874,
        WmfDibBitBlt = 67904,
        WmfExtTextOut = 68146,
        WmfStretchBlt = 68387,
        WmfDibStretchBlt = 68417,
        WmfSetDibToDev = 68915,
        WmfStretchDib = 69443
    }

    public enum EmfType
    {
        EmfOnly = 3,
        EmfPlusOnly = 4,
        EmfPlusDual = 5
    }

    public sealed partial class Encoder
    {
        public static readonly Encoder ChrominanceTable;
        public static readonly Encoder ColorDepth;
        public static readonly Encoder ColorSpace;
        public static readonly Encoder Compression;
        public static readonly Encoder ImageItems;
        public static readonly Encoder LuminanceTable;
        public static readonly Encoder Quality;
        public static readonly Encoder RenderMethod;
        public static readonly Encoder SaveAsCmyk;
        public static readonly Encoder SaveFlag;
        public static readonly Encoder ScanMethod;
        public static readonly Encoder Transformation;
        public static readonly Encoder Version;
        public Encoder(Guid guid) { }

        public Guid Guid { get { throw null; } }
    }

    public sealed partial class EncoderParameter : IDisposable
    {
        public EncoderParameter(Encoder encoder, byte value, bool undefined) { }

        public EncoderParameter(Encoder encoder, byte value) { }

        public EncoderParameter(Encoder encoder, byte[] value, bool undefined) { }

        public EncoderParameter(Encoder encoder, byte[] value) { }

        public EncoderParameter(Encoder encoder, short value) { }

        public EncoderParameter(Encoder encoder, short[] value) { }

        public EncoderParameter(Encoder encoder, int numberValues, EncoderParameterValueType type, nint value) { }

        public EncoderParameter(Encoder encoder, int numerator1, int demoninator1, int numerator2, int demoninator2) { }

        [Obsolete("This constructor has been deprecated. Use EncoderParameter(Encoder encoder, int numberValues, EncoderParameterValueType type, IntPtr value) instead.")]
        public EncoderParameter(Encoder encoder, int NumberOfValues, int Type, int Value) { }

        public EncoderParameter(Encoder encoder, int numerator, int denominator) { }

        public EncoderParameter(Encoder encoder, int[] numerator1, int[] denominator1, int[] numerator2, int[] denominator2) { }

        public EncoderParameter(Encoder encoder, int[] numerator, int[] denominator) { }

        public EncoderParameter(Encoder encoder, long rangebegin, long rangeend) { }

        public EncoderParameter(Encoder encoder, long value) { }

        public EncoderParameter(Encoder encoder, long[] rangebegin, long[] rangeend) { }

        public EncoderParameter(Encoder encoder, long[] value) { }

        public EncoderParameter(Encoder encoder, string value) { }

        public Encoder Encoder { get { throw null; } set { } }

        public int NumberOfValues { get { throw null; } }

        public EncoderParameterValueType Type { get { throw null; } }

        public EncoderParameterValueType ValueType { get { throw null; } }

        public void Dispose() { }

        ~EncoderParameter() {
        }
    }

    public sealed partial class EncoderParameters : IDisposable
    {
        public EncoderParameters() { }

        public EncoderParameters(int count) { }

        public EncoderParameter[] Param { get { throw null; } set { } }

        public void Dispose() { }
    }

    public enum EncoderParameterValueType
    {
        ValueTypeByte = 1,
        ValueTypeAscii = 2,
        ValueTypeShort = 3,
        ValueTypeLong = 4,
        ValueTypeRational = 5,
        ValueTypeLongRange = 6,
        ValueTypeUndefined = 7,
        ValueTypeRationalRange = 8,
        ValueTypePointer = 9
    }

    public enum EncoderValue
    {
        ColorTypeCMYK = 0,
        ColorTypeYCCK = 1,
        CompressionLZW = 2,
        CompressionCCITT3 = 3,
        CompressionCCITT4 = 4,
        CompressionRle = 5,
        CompressionNone = 6,
        ScanMethodInterlaced = 7,
        ScanMethodNonInterlaced = 8,
        VersionGif87 = 9,
        VersionGif89 = 10,
        RenderProgressive = 11,
        RenderNonProgressive = 12,
        TransformRotate90 = 13,
        TransformRotate180 = 14,
        TransformRotate270 = 15,
        TransformFlipHorizontal = 16,
        TransformFlipVertical = 17,
        MultiFrame = 18,
        LastFrame = 19,
        Flush = 20,
        FrameDimensionTime = 21,
        FrameDimensionResolution = 22,
        FrameDimensionPage = 23
    }

    public sealed partial class FrameDimension
    {
        public FrameDimension(Guid guid) { }

        public Guid Guid { get { throw null; } }

        public static FrameDimension Page { get { throw null; } }

        public static FrameDimension Resolution { get { throw null; } }

        public static FrameDimension Time { get { throw null; } }

        public override bool Equals(object? o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class ImageAttributes : ICloneable, IDisposable
    {
        public void ClearBrushRemapTable() { }

        public void ClearColorKey() { }

        public void ClearColorKey(ColorAdjustType type) { }

        public void ClearColorMatrix() { }

        public void ClearColorMatrix(ColorAdjustType type) { }

        public void ClearGamma() { }

        public void ClearGamma(ColorAdjustType type) { }

        public void ClearNoOp() { }

        public void ClearNoOp(ColorAdjustType type) { }

        public void ClearOutputChannel() { }

        public void ClearOutputChannel(ColorAdjustType type) { }

        public void ClearOutputChannelColorProfile() { }

        public void ClearOutputChannelColorProfile(ColorAdjustType type) { }

        public void ClearRemapTable() { }

        public void ClearRemapTable(ColorAdjustType type) { }

        public void ClearThreshold() { }

        public void ClearThreshold(ColorAdjustType type) { }

        public object Clone() { throw null; }

        public void Dispose() { }

        ~ImageAttributes() {
        }

        public void GetAdjustedPalette(ColorPalette palette, ColorAdjustType type) { }

        public void SetBrushRemapTable(ColorMap[] map) { }

        public void SetColorKey(Color colorLow, Color colorHigh, ColorAdjustType type) { }

        public void SetColorKey(Color colorLow, Color colorHigh) { }

        public void SetColorMatrices(ColorMatrix newColorMatrix, ColorMatrix? grayMatrix, ColorMatrixFlag mode, ColorAdjustType type) { }

        public void SetColorMatrices(ColorMatrix newColorMatrix, ColorMatrix? grayMatrix, ColorMatrixFlag flags) { }

        public void SetColorMatrices(ColorMatrix newColorMatrix, ColorMatrix? grayMatrix) { }

        public void SetColorMatrix(ColorMatrix newColorMatrix, ColorMatrixFlag mode, ColorAdjustType type) { }

        public void SetColorMatrix(ColorMatrix newColorMatrix, ColorMatrixFlag flags) { }

        public void SetColorMatrix(ColorMatrix newColorMatrix) { }

        public void SetGamma(float gamma, ColorAdjustType type) { }

        public void SetGamma(float gamma) { }

        public void SetNoOp() { }

        public void SetNoOp(ColorAdjustType type) { }

        public void SetOutputChannel(ColorChannelFlag flags, ColorAdjustType type) { }

        public void SetOutputChannel(ColorChannelFlag flags) { }

        public void SetOutputChannelColorProfile(string colorProfileFilename, ColorAdjustType type) { }

        public void SetOutputChannelColorProfile(string colorProfileFilename) { }

        public void SetRemapTable(ColorMap[] map, ColorAdjustType type) { }

        public void SetRemapTable(ColorMap[] map) { }

        public void SetThreshold(float threshold, ColorAdjustType type) { }

        public void SetThreshold(float threshold) { }

        public void SetWrapMode(Drawing2D.WrapMode mode, Color color, bool clamp) { }

        public void SetWrapMode(Drawing2D.WrapMode mode, Color color) { }

        public void SetWrapMode(Drawing2D.WrapMode mode) { }
    }

    [Flags]
    public enum ImageCodecFlags
    {
        Encoder = 1,
        Decoder = 2,
        SupportBitmap = 4,
        SupportVector = 8,
        SeekableEncode = 16,
        BlockingDecode = 32,
        Builtin = 65536,
        System = 131072,
        User = 262144
    }

    public sealed partial class ImageCodecInfo
    {
        internal ImageCodecInfo() { }

        public Guid Clsid { get { throw null; } set { } }

        public string? CodecName { get { throw null; } set { } }

        public string? DllName { get { throw null; } set { } }

        public string? FilenameExtension { get { throw null; } set { } }

        public ImageCodecFlags Flags { get { throw null; } set { } }

        public string? FormatDescription { get { throw null; } set { } }

        public Guid FormatID { get { throw null; } set { } }

        public string? MimeType { get { throw null; } set { } }

        [CLSCompliant(false)]
        public byte[][]? SignatureMasks { get { throw null; } set { } }

        [CLSCompliant(false)]
        public byte[][]? SignaturePatterns { get { throw null; } set { } }

        public int Version { get { throw null; } set { } }

        public static ImageCodecInfo[] GetImageDecoders() { throw null; }

        public static ImageCodecInfo[] GetImageEncoders() { throw null; }
    }

    [Flags]
    public enum ImageFlags
    {
        None = 0,
        Scalable = 1,
        HasAlpha = 2,
        HasTranslucent = 4,
        PartiallyScalable = 8,
        ColorSpaceRgb = 16,
        ColorSpaceCmyk = 32,
        ColorSpaceGray = 64,
        ColorSpaceYcbcr = 128,
        ColorSpaceYcck = 256,
        HasRealDpi = 4096,
        HasRealPixelSize = 8192,
        ReadOnly = 65536,
        Caching = 131072
    }

    public sealed partial class ImageFormat
    {
        public ImageFormat(Guid guid) { }

        public static ImageFormat Bmp { get { throw null; } }

        public static ImageFormat Emf { get { throw null; } }

        public static ImageFormat Exif { get { throw null; } }

        public static ImageFormat Gif { get { throw null; } }

        public Guid Guid { get { throw null; } }

        [Runtime.Versioning.SupportedOSPlatform("windows10.0.17763.0")]
        public static ImageFormat Heif { get { throw null; } }

        public static ImageFormat Icon { get { throw null; } }

        public static ImageFormat Jpeg { get { throw null; } }

        public static ImageFormat MemoryBmp { get { throw null; } }

        public static ImageFormat Png { get { throw null; } }

        public static ImageFormat Tiff { get { throw null; } }

        [Runtime.Versioning.SupportedOSPlatform("windows10.0.17763.0")]
        public static ImageFormat Webp { get { throw null; } }

        public static ImageFormat Wmf { get { throw null; } }

        public override bool Equals(object? o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public enum ImageLockMode
    {
        ReadOnly = 1,
        WriteOnly = 2,
        ReadWrite = 3,
        UserInputBuffer = 4
    }

    public sealed partial class Metafile : Image
    {
        public Metafile(nint henhmetafile, bool deleteEmf) { }

        public Metafile(nint referenceHdc, EmfType emfType, string? description) { }

        public Metafile(nint referenceHdc, EmfType emfType) { }

        public Metafile(nint hmetafile, WmfPlaceableFileHeader wmfHeader, bool deleteWmf) { }

        public Metafile(nint hmetafile, WmfPlaceableFileHeader wmfHeader) { }

        public Metafile(nint referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type, string? desc) { }

        public Metafile(nint referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type) { }

        public Metafile(nint referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit) { }

        public Metafile(nint referenceHdc, Rectangle frameRect) { }

        public Metafile(nint referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type, string? description) { }

        public Metafile(nint referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type) { }

        public Metafile(nint referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit) { }

        public Metafile(nint referenceHdc, RectangleF frameRect) { }

        public Metafile(IO.Stream stream, nint referenceHdc, EmfType type, string? description) { }

        public Metafile(IO.Stream stream, nint referenceHdc, EmfType type) { }

        public Metafile(IO.Stream stream, nint referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type, string? description) { }

        public Metafile(IO.Stream stream, nint referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type) { }

        public Metafile(IO.Stream stream, nint referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit) { }

        public Metafile(IO.Stream stream, nint referenceHdc, Rectangle frameRect) { }

        public Metafile(IO.Stream stream, nint referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type, string? description) { }

        public Metafile(IO.Stream stream, nint referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type) { }

        public Metafile(IO.Stream stream, nint referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit) { }

        public Metafile(IO.Stream stream, nint referenceHdc, RectangleF frameRect) { }

        public Metafile(IO.Stream stream, nint referenceHdc) { }

        public Metafile(IO.Stream stream) { }

        public Metafile(string fileName, nint referenceHdc, EmfType type, string? description) { }

        public Metafile(string fileName, nint referenceHdc, EmfType type) { }

        public Metafile(string fileName, nint referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type, string? description) { }

        public Metafile(string fileName, nint referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type) { }

        public Metafile(string fileName, nint referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, string? description) { }

        public Metafile(string fileName, nint referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit) { }

        public Metafile(string fileName, nint referenceHdc, Rectangle frameRect) { }

        public Metafile(string fileName, nint referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type, string? description) { }

        public Metafile(string fileName, nint referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type) { }

        public Metafile(string fileName, nint referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, string? desc) { }

        public Metafile(string fileName, nint referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit) { }

        public Metafile(string fileName, nint referenceHdc, RectangleF frameRect) { }

        public Metafile(string fileName, nint referenceHdc) { }

        public Metafile(string filename) { }

        public nint GetHenhmetafile() { throw null; }

        public MetafileHeader GetMetafileHeader() { throw null; }

        public static MetafileHeader GetMetafileHeader(nint hmetafile, WmfPlaceableFileHeader wmfHeader) { throw null; }

        public static MetafileHeader GetMetafileHeader(nint henhmetafile) { throw null; }

        public static MetafileHeader GetMetafileHeader(IO.Stream stream) { throw null; }

        public static MetafileHeader GetMetafileHeader(string fileName) { throw null; }

        public void PlayRecord(EmfPlusRecordType recordType, int flags, int dataSize, byte[] data) { }
    }

    public enum MetafileFrameUnit
    {
        Pixel = 2,
        Point = 3,
        Inch = 4,
        Document = 5,
        Millimeter = 6,
        GdiCompatible = 7
    }

    public sealed partial class MetafileHeader
    {
        internal MetafileHeader() { }

        public Rectangle Bounds { get { throw null; } }

        public float DpiX { get { throw null; } }

        public float DpiY { get { throw null; } }

        public int EmfPlusHeaderSize { get { throw null; } }

        public int LogicalDpiX { get { throw null; } }

        public int LogicalDpiY { get { throw null; } }

        public int MetafileSize { get { throw null; } }

        public MetafileType Type { get { throw null; } }

        public int Version { get { throw null; } }

        public MetaHeader WmfHeader { get { throw null; } }

        public bool IsDisplay() { throw null; }

        public bool IsEmf() { throw null; }

        public bool IsEmfOrEmfPlus() { throw null; }

        public bool IsEmfPlus() { throw null; }

        public bool IsEmfPlusDual() { throw null; }

        public bool IsEmfPlusOnly() { throw null; }

        public bool IsWmf() { throw null; }

        public bool IsWmfPlaceable() { throw null; }
    }

    public enum MetafileType
    {
        Invalid = 0,
        Wmf = 1,
        WmfPlaceable = 2,
        Emf = 3,
        EmfPlusOnly = 4,
        EmfPlusDual = 5
    }

    public sealed partial class MetaHeader
    {
        public short HeaderSize { get { throw null; } set { } }

        public int MaxRecord { get { throw null; } set { } }

        public short NoObjects { get { throw null; } set { } }

        public short NoParameters { get { throw null; } set { } }

        public int Size { get { throw null; } set { } }

        public short Type { get { throw null; } set { } }

        public short Version { get { throw null; } set { } }
    }

    [Flags]
    public enum PaletteFlags
    {
        HasAlpha = 1,
        GrayScale = 2,
        Halftone = 4
    }

    public enum PixelFormat
    {
        DontCare = 0,
        Undefined = 0,
        Max = 15,
        Indexed = 65536,
        Gdi = 131072,
        Format16bppRgb555 = 135173,
        Format16bppRgb565 = 135174,
        Format24bppRgb = 137224,
        Format32bppRgb = 139273,
        Format1bppIndexed = 196865,
        Format4bppIndexed = 197634,
        Format8bppIndexed = 198659,
        Alpha = 262144,
        Format16bppArgb1555 = 397319,
        PAlpha = 524288,
        Format32bppPArgb = 925707,
        Extended = 1048576,
        Format16bppGrayScale = 1052676,
        Format48bppRgb = 1060876,
        Format64bppPArgb = 1851406,
        Canonical = 2097152,
        Format32bppArgb = 2498570,
        Format64bppArgb = 3424269
    }

    public delegate void PlayRecordCallback(EmfPlusRecordType recordType, int flags, int dataSize, nint recordData);
    public sealed partial class PropertyItem
    {
        internal PropertyItem() { }

        public int Id { get { throw null; } set { } }

        public int Len { get { throw null; } set { } }

        public short Type { get { throw null; } set { } }

        public byte[]? Value { get { throw null; } set { } }
    }

    public sealed partial class WmfPlaceableFileHeader
    {
        public short BboxBottom { get { throw null; } set { } }

        public short BboxLeft { get { throw null; } set { } }

        public short BboxRight { get { throw null; } set { } }

        public short BboxTop { get { throw null; } set { } }

        public short Checksum { get { throw null; } set { } }

        public short Hmf { get { throw null; } set { } }

        public short Inch { get { throw null; } set { } }

        public int Key { get { throw null; } set { } }

        public int Reserved { get { throw null; } set { } }
    }
}

namespace System.Drawing.Interop
{
    public partial struct LOGFONT
    {
        private int _dummyPrimitive;
        public byte lfCharSet;
        public byte lfClipPrecision;
        public int lfEscapement;
        public int lfHeight;
        public byte lfItalic;
        public int lfOrientation;
        public byte lfOutPrecision;
        public byte lfPitchAndFamily;
        public byte lfQuality;
        public byte lfStrikeOut;
        public byte lfUnderline;
        public int lfWeight;
        public int lfWidth;
        [Diagnostics.CodeAnalysis.UnscopedRef]
        public Span<char> lfFaceName { get { throw null; } }
    }
}

namespace System.Drawing.Printing
{
    public enum Duplex
    {
        Default = -1,
        Simplex = 1,
        Vertical = 2,
        Horizontal = 3
    }

    public partial class InvalidPrinterException : SystemException
    {
        public InvalidPrinterException(PrinterSettings settings) { }

        [Obsolete(DiagnosticId = "SYSLIB0051")]
        protected InvalidPrinterException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        [Obsolete(DiagnosticId = "SYSLIB0051")]
        public override void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }
    }

    public partial class Margins : ICloneable
    {
        public Margins() { }

        public Margins(int left, int right, int top, int bottom) { }

        public int Bottom { get { throw null; } set { } }

        public int Left { get { throw null; } set { } }

        public int Right { get { throw null; } set { } }

        public int Top { get { throw null; } set { } }

        public object Clone() { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(Margins? m1, Margins? m2) { throw null; }

        public static bool operator !=(Margins? m1, Margins? m2) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class MarginsConverter : ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext? context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext? context, Type? destinationType) { throw null; }

        public override object? ConvertFrom(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object value) { throw null; }

        public override object? ConvertTo(ComponentModel.ITypeDescriptorContext? context, Globalization.CultureInfo? culture, object? value, Type destinationType) { throw null; }

        public override object CreateInstance(ComponentModel.ITypeDescriptorContext? context, Collections.IDictionary propertyValues) { throw null; }

        public override bool GetCreateInstanceSupported(ComponentModel.ITypeDescriptorContext? context) { throw null; }
    }

    public partial class PageSettings : ICloneable
    {
        public PageSettings() { }

        public PageSettings(PrinterSettings printerSettings) { }

        public Rectangle Bounds { get { throw null; } }

        public bool Color { get { throw null; } set { } }

        public float HardMarginX { get { throw null; } }

        public float HardMarginY { get { throw null; } }

        public bool Landscape { get { throw null; } set { } }

        public Margins Margins { get { throw null; } set { } }

        public PaperSize PaperSize { get { throw null; } set { } }

        public PaperSource PaperSource { get { throw null; } set { } }

        public RectangleF PrintableArea { get { throw null; } }

        public PrinterResolution PrinterResolution { get { throw null; } set { } }

        public PrinterSettings PrinterSettings { get { throw null; } set { } }

        public object Clone() { throw null; }

        public void CopyToHdevmode(nint hdevmode) { }

        public void SetHdevmode(nint hdevmode) { }

        public override string ToString() { throw null; }
    }

    public enum PaperKind
    {
        Custom = 0,
        Letter = 1,
        LetterSmall = 2,
        Tabloid = 3,
        Ledger = 4,
        Legal = 5,
        Statement = 6,
        Executive = 7,
        A3 = 8,
        A4 = 9,
        A4Small = 10,
        A5 = 11,
        B4 = 12,
        B5 = 13,
        Folio = 14,
        Quarto = 15,
        Standard10x14 = 16,
        Standard11x17 = 17,
        Note = 18,
        Number9Envelope = 19,
        Number10Envelope = 20,
        Number11Envelope = 21,
        Number12Envelope = 22,
        Number14Envelope = 23,
        CSheet = 24,
        DSheet = 25,
        ESheet = 26,
        DLEnvelope = 27,
        C5Envelope = 28,
        C3Envelope = 29,
        C4Envelope = 30,
        C6Envelope = 31,
        C65Envelope = 32,
        B4Envelope = 33,
        B5Envelope = 34,
        B6Envelope = 35,
        ItalyEnvelope = 36,
        MonarchEnvelope = 37,
        PersonalEnvelope = 38,
        USStandardFanfold = 39,
        GermanStandardFanfold = 40,
        GermanLegalFanfold = 41,
        IsoB4 = 42,
        JapanesePostcard = 43,
        Standard9x11 = 44,
        Standard10x11 = 45,
        Standard15x11 = 46,
        InviteEnvelope = 47,
        LetterExtra = 50,
        LegalExtra = 51,
        TabloidExtra = 52,
        A4Extra = 53,
        LetterTransverse = 54,
        A4Transverse = 55,
        LetterExtraTransverse = 56,
        APlus = 57,
        BPlus = 58,
        LetterPlus = 59,
        A4Plus = 60,
        A5Transverse = 61,
        B5Transverse = 62,
        A3Extra = 63,
        A5Extra = 64,
        B5Extra = 65,
        A2 = 66,
        A3Transverse = 67,
        A3ExtraTransverse = 68,
        JapaneseDoublePostcard = 69,
        A6 = 70,
        JapaneseEnvelopeKakuNumber2 = 71,
        JapaneseEnvelopeKakuNumber3 = 72,
        JapaneseEnvelopeChouNumber3 = 73,
        JapaneseEnvelopeChouNumber4 = 74,
        LetterRotated = 75,
        A3Rotated = 76,
        A4Rotated = 77,
        A5Rotated = 78,
        B4JisRotated = 79,
        B5JisRotated = 80,
        JapanesePostcardRotated = 81,
        JapaneseDoublePostcardRotated = 82,
        A6Rotated = 83,
        JapaneseEnvelopeKakuNumber2Rotated = 84,
        JapaneseEnvelopeKakuNumber3Rotated = 85,
        JapaneseEnvelopeChouNumber3Rotated = 86,
        JapaneseEnvelopeChouNumber4Rotated = 87,
        B6Jis = 88,
        B6JisRotated = 89,
        Standard12x11 = 90,
        JapaneseEnvelopeYouNumber4 = 91,
        JapaneseEnvelopeYouNumber4Rotated = 92,
        Prc16K = 93,
        Prc32K = 94,
        Prc32KBig = 95,
        PrcEnvelopeNumber1 = 96,
        PrcEnvelopeNumber2 = 97,
        PrcEnvelopeNumber3 = 98,
        PrcEnvelopeNumber4 = 99,
        PrcEnvelopeNumber5 = 100,
        PrcEnvelopeNumber6 = 101,
        PrcEnvelopeNumber7 = 102,
        PrcEnvelopeNumber8 = 103,
        PrcEnvelopeNumber9 = 104,
        PrcEnvelopeNumber10 = 105,
        Prc16KRotated = 106,
        Prc32KRotated = 107,
        Prc32KBigRotated = 108,
        PrcEnvelopeNumber1Rotated = 109,
        PrcEnvelopeNumber2Rotated = 110,
        PrcEnvelopeNumber3Rotated = 111,
        PrcEnvelopeNumber4Rotated = 112,
        PrcEnvelopeNumber5Rotated = 113,
        PrcEnvelopeNumber6Rotated = 114,
        PrcEnvelopeNumber7Rotated = 115,
        PrcEnvelopeNumber8Rotated = 116,
        PrcEnvelopeNumber9Rotated = 117,
        PrcEnvelopeNumber10Rotated = 118
    }

    public partial class PaperSize
    {
        public PaperSize() { }

        public PaperSize(string name, int width, int height) { }

        public int Height { get { throw null; } set { } }

        public PaperKind Kind { get { throw null; } }

        public string PaperName { get { throw null; } set { } }

        public int RawKind { get { throw null; } set { } }

        public int Width { get { throw null; } set { } }

        public override string ToString() { throw null; }
    }

    public partial class PaperSource
    {
        public PaperSourceKind Kind { get { throw null; } }

        public int RawKind { get { throw null; } set { } }

        public string SourceName { get { throw null; } set { } }

        public override string ToString() { throw null; }
    }

    public enum PaperSourceKind
    {
        Upper = 1,
        Lower = 2,
        Middle = 3,
        Manual = 4,
        Envelope = 5,
        ManualFeed = 6,
        AutomaticFeed = 7,
        TractorFeed = 8,
        SmallFormat = 9,
        LargeFormat = 10,
        LargeCapacity = 11,
        Cassette = 14,
        FormSource = 15,
        Custom = 257
    }

    public sealed partial class PreviewPageInfo
    {
        public PreviewPageInfo(Image image, Size physicalSize) { }

        public Image Image { get { throw null; } }

        public Size PhysicalSize { get { throw null; } }
    }

    public partial class PreviewPrintController : PrintController
    {
        public override bool IsPreview { get { throw null; } }

        public virtual bool UseAntiAlias { get { throw null; } set { } }

        public PreviewPageInfo[] GetPreviewPageInfo() { throw null; }

        public override void OnEndPage(PrintDocument document, PrintPageEventArgs e) { }

        public override void OnEndPrint(PrintDocument document, PrintEventArgs e) { }

        public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e) { throw null; }

        public override void OnStartPrint(PrintDocument document, PrintEventArgs e) { }
    }

    public enum PrintAction
    {
        PrintToFile = 0,
        PrintToPreview = 1,
        PrintToPrinter = 2
    }

    public abstract partial class PrintController
    {
        public virtual bool IsPreview { get { throw null; } }

        public virtual void OnEndPage(PrintDocument document, PrintPageEventArgs e) { }

        public virtual void OnEndPrint(PrintDocument document, PrintEventArgs e) { }

        public virtual Graphics? OnStartPage(PrintDocument document, PrintPageEventArgs e) { throw null; }

        public virtual void OnStartPrint(PrintDocument document, PrintEventArgs e) { }
    }

    [ComponentModel.DefaultProperty("DocumentName")]
    [ComponentModel.DefaultEvent("PrintPage")]
    public partial class PrintDocument : ComponentModel.Component
    {
        [ComponentModel.Browsable(false)]
        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public PageSettings DefaultPageSettings { get { throw null; } set { } }

        [ComponentModel.DefaultValue("document")]
        public string DocumentName { get { throw null; } set { } }

        [ComponentModel.DefaultValue(false)]
        public bool OriginAtMargins { get { throw null; } set { } }

        [ComponentModel.Browsable(false)]
        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public PrintController PrintController { get { throw null; } set { } }

        [ComponentModel.Browsable(false)]
        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public PrinterSettings PrinterSettings { get { throw null; } set { } }

        public event PrintEventHandler BeginPrint { add { } remove { } }

        public event PrintEventHandler EndPrint { add { } remove { } }

        public event PrintPageEventHandler PrintPage { add { } remove { } }

        public event QueryPageSettingsEventHandler QueryPageSettings { add { } remove { } }

        protected internal virtual void OnBeginPrint(PrintEventArgs e) { }

        protected internal virtual void OnEndPrint(PrintEventArgs e) { }

        protected internal virtual void OnPrintPage(PrintPageEventArgs e) { }

        protected internal virtual void OnQueryPageSettings(QueryPageSettingsEventArgs e) { }

        public void Print() { }

        public override string ToString() { throw null; }
    }

    public partial class PrinterResolution
    {
        public PrinterResolutionKind Kind { get { throw null; } set { } }

        public int X { get { throw null; } set { } }

        public int Y { get { throw null; } set { } }

        public override string ToString() { throw null; }
    }

    public enum PrinterResolutionKind
    {
        High = -4,
        Medium = -3,
        Low = -2,
        Draft = -1,
        Custom = 0
    }

    public partial class PrinterSettings : ICloneable
    {
        public bool CanDuplex { get { throw null; } }

        public bool Collate { get { throw null; } set { } }

        public short Copies { get { throw null; } set { } }

        public PageSettings DefaultPageSettings { get { throw null; } }

        public Duplex Duplex { get { throw null; } set { } }

        public int FromPage { get { throw null; } set { } }

        public static StringCollection InstalledPrinters { get { throw null; } }

        public bool IsDefaultPrinter { get { throw null; } }

        public bool IsPlotter { get { throw null; } }

        public bool IsValid { get { throw null; } }

        public int LandscapeAngle { get { throw null; } }

        public int MaximumCopies { get { throw null; } }

        public int MaximumPage { get { throw null; } set { } }

        public int MinimumPage { get { throw null; } set { } }

        public PaperSizeCollection PaperSizes { get { throw null; } }

        public PaperSourceCollection PaperSources { get { throw null; } }

        public string PrinterName { get { throw null; } set { } }

        public PrinterResolutionCollection PrinterResolutions { get { throw null; } }

        public string PrintFileName { get { throw null; } set { } }

        public PrintRange PrintRange { get { throw null; } set { } }

        public bool PrintToFile { get { throw null; } set { } }

        public bool SupportsColor { get { throw null; } }

        public int ToPage { get { throw null; } set { } }

        public object Clone() { throw null; }

        public Graphics CreateMeasurementGraphics() { throw null; }

        public Graphics CreateMeasurementGraphics(bool honorOriginAtMargins) { throw null; }

        public Graphics CreateMeasurementGraphics(PageSettings pageSettings, bool honorOriginAtMargins) { throw null; }

        public Graphics CreateMeasurementGraphics(PageSettings pageSettings) { throw null; }

        public nint GetHdevmode() { throw null; }

        public nint GetHdevmode(PageSettings pageSettings) { throw null; }

        public nint GetHdevnames() { throw null; }

        public bool IsDirectPrintingSupported(Image image) { throw null; }

        public bool IsDirectPrintingSupported(Imaging.ImageFormat imageFormat) { throw null; }

        public void SetHdevmode(nint hdevmode) { }

        public void SetHdevnames(nint hdevnames) { }

        public override string ToString() { throw null; }

        public partial class PaperSizeCollection : Collections.ICollection, Collections.IEnumerable
        {
            public PaperSizeCollection(PaperSize[] array) { }

            public int Count { get { throw null; } }

            public virtual PaperSize this[int index] { get { throw null; } }

            int Collections.ICollection.Count { get { throw null; } }

            bool Collections.ICollection.IsSynchronized { get { throw null; } }

            object Collections.ICollection.SyncRoot { get { throw null; } }

            public int Add(PaperSize paperSize) { throw null; }

            public void CopyTo(PaperSize[] paperSizes, int index) { }

            public Collections.IEnumerator GetEnumerator() { throw null; }

            void Collections.ICollection.CopyTo(Array array, int index) { }

            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
        }

        public partial class PaperSourceCollection : Collections.ICollection, Collections.IEnumerable
        {
            public PaperSourceCollection(PaperSource[] array) { }

            public int Count { get { throw null; } }

            public virtual PaperSource this[int index] { get { throw null; } }

            int Collections.ICollection.Count { get { throw null; } }

            bool Collections.ICollection.IsSynchronized { get { throw null; } }

            object Collections.ICollection.SyncRoot { get { throw null; } }

            public int Add(PaperSource paperSource) { throw null; }

            public void CopyTo(PaperSource[] paperSources, int index) { }

            public Collections.IEnumerator GetEnumerator() { throw null; }

            void Collections.ICollection.CopyTo(Array array, int index) { }

            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
        }

        public partial class PrinterResolutionCollection : Collections.ICollection, Collections.IEnumerable
        {
            public PrinterResolutionCollection(PrinterResolution[] array) { }

            public int Count { get { throw null; } }

            public virtual PrinterResolution this[int index] { get { throw null; } }

            int Collections.ICollection.Count { get { throw null; } }

            bool Collections.ICollection.IsSynchronized { get { throw null; } }

            object Collections.ICollection.SyncRoot { get { throw null; } }

            public int Add(PrinterResolution printerResolution) { throw null; }

            public void CopyTo(PrinterResolution[] printerResolutions, int index) { }

            public Collections.IEnumerator GetEnumerator() { throw null; }

            void Collections.ICollection.CopyTo(Array array, int index) { }

            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
        }

        public partial class StringCollection : Collections.ICollection, Collections.IEnumerable
        {
            public StringCollection(string[] array) { }

            public int Count { get { throw null; } }

            public virtual string this[int index] { get { throw null; } }

            int Collections.ICollection.Count { get { throw null; } }

            bool Collections.ICollection.IsSynchronized { get { throw null; } }

            object Collections.ICollection.SyncRoot { get { throw null; } }

            public int Add(string value) { throw null; }

            public void CopyTo(string[] strings, int index) { }

            public Collections.IEnumerator GetEnumerator() { throw null; }

            void Collections.ICollection.CopyTo(Array array, int index) { }

            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
        }
    }

    public enum PrinterUnit
    {
        Display = 0,
        ThousandthsOfAnInch = 1,
        HundredthsOfAMillimeter = 2,
        TenthsOfAMillimeter = 3
    }

    public sealed partial class PrinterUnitConvert
    {
        internal PrinterUnitConvert() { }

        public static double Convert(double value, PrinterUnit fromUnit, PrinterUnit toUnit) { throw null; }

        public static Point Convert(Point value, PrinterUnit fromUnit, PrinterUnit toUnit) { throw null; }

        public static Margins Convert(Margins value, PrinterUnit fromUnit, PrinterUnit toUnit) { throw null; }

        public static Rectangle Convert(Rectangle value, PrinterUnit fromUnit, PrinterUnit toUnit) { throw null; }

        public static Size Convert(Size value, PrinterUnit fromUnit, PrinterUnit toUnit) { throw null; }

        public static int Convert(int value, PrinterUnit fromUnit, PrinterUnit toUnit) { throw null; }
    }

    public partial class PrintEventArgs : ComponentModel.CancelEventArgs
    {
        public PrintAction PrintAction { get { throw null; } }
    }

    public delegate void PrintEventHandler(object sender, PrintEventArgs e);
    public partial class PrintPageEventArgs : EventArgs
    {
        public PrintPageEventArgs(Graphics? graphics, Rectangle marginBounds, Rectangle pageBounds, PageSettings pageSettings) { }

        public bool Cancel { get { throw null; } set { } }

        public Graphics? Graphics { get { throw null; } }

        public bool HasMorePages { get { throw null; } set { } }

        public Rectangle MarginBounds { get { throw null; } }

        public Rectangle PageBounds { get { throw null; } }

        public PageSettings PageSettings { get { throw null; } }
    }

    public delegate void PrintPageEventHandler(object sender, PrintPageEventArgs e);
    public enum PrintRange
    {
        AllPages = 0,
        Selection = 1,
        SomePages = 2,
        CurrentPage = 4194304
    }

    public partial class QueryPageSettingsEventArgs : PrintEventArgs
    {
        public QueryPageSettingsEventArgs(PageSettings pageSettings) { }

        public PageSettings PageSettings { get { throw null; } set { } }
    }

    public delegate void QueryPageSettingsEventHandler(object sender, QueryPageSettingsEventArgs e);
    public partial class StandardPrintController : PrintController
    {
        public override void OnEndPage(PrintDocument document, PrintPageEventArgs e) { }

        public override void OnEndPrint(PrintDocument document, PrintEventArgs e) { }

        public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e) { throw null; }

        public override void OnStartPrint(PrintDocument document, PrintEventArgs e) { }
    }
}

namespace System.Drawing.Text
{
    public abstract partial class FontCollection : IDisposable
    {
        internal FontCollection() { }

        public FontFamily[] Families { get { throw null; } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~FontCollection() {
        }
    }

    public enum GenericFontFamilies
    {
        Serif = 0,
        SansSerif = 1,
        Monospace = 2
    }

    public enum HotkeyPrefix
    {
        None = 0,
        Show = 1,
        Hide = 2
    }

    public sealed partial class InstalledFontCollection : FontCollection
    {
        public InstalledFontCollection() { }
    }

    public sealed partial class PrivateFontCollection : FontCollection
    {
        public PrivateFontCollection() { }

        public void AddFontFile(string filename) { }

        public void AddMemoryFont(nint memory, int length) { }

        protected override void Dispose(bool disposing) { }
    }

    public enum TextRenderingHint
    {
        SystemDefault = 0,
        SingleBitPerPixelGridFit = 1,
        SingleBitPerPixel = 2,
        AntiAliasGridFit = 3,
        AntiAlias = 4,
        ClearTypeGridFit = 5
    }
}