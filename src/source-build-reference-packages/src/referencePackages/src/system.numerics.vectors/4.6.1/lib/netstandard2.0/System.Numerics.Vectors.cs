// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Numerics.Vectors")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Numerics.Vectors")]
[assembly: System.Reflection.AssemblyFileVersion("4.600.125.16908")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.1+6b84308c9ad012f53240d72c1d716d7e42546483")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Numerics.Vectors")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/maintenance-packages")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.3.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Numerics
{
    public partial struct Matrix3x2 : IEquatable<Matrix3x2>
    {
        public float M11;
        public float M12;
        public float M21;
        public float M22;
        public float M31;
        public float M32;
        public Matrix3x2(float m11, float m12, float m21, float m22, float m31, float m32) { }

        public static Matrix3x2 Identity { get { throw null; } }

        public bool IsIdentity { get { throw null; } }

        public Vector2 Translation { get { throw null; } set { } }

        public static Matrix3x2 Add(Matrix3x2 value1, Matrix3x2 value2) { throw null; }

        public static Matrix3x2 CreateRotation(float radians, Vector2 centerPoint) { throw null; }

        public static Matrix3x2 CreateRotation(float radians) { throw null; }

        public static Matrix3x2 CreateScale(Vector2 scales, Vector2 centerPoint) { throw null; }

        public static Matrix3x2 CreateScale(Vector2 scales) { throw null; }

        public static Matrix3x2 CreateScale(float scale, Vector2 centerPoint) { throw null; }

        public static Matrix3x2 CreateScale(float xScale, float yScale, Vector2 centerPoint) { throw null; }

        public static Matrix3x2 CreateScale(float xScale, float yScale) { throw null; }

        public static Matrix3x2 CreateScale(float scale) { throw null; }

        public static Matrix3x2 CreateSkew(float radiansX, float radiansY, Vector2 centerPoint) { throw null; }

        public static Matrix3x2 CreateSkew(float radiansX, float radiansY) { throw null; }

        public static Matrix3x2 CreateTranslation(Vector2 position) { throw null; }

        public static Matrix3x2 CreateTranslation(float xPosition, float yPosition) { throw null; }

        public bool Equals(Matrix3x2 other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public float GetDeterminant() { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool Invert(Matrix3x2 matrix, out Matrix3x2 result) { throw null; }

        public static Matrix3x2 Lerp(Matrix3x2 matrix1, Matrix3x2 matrix2, float amount) { throw null; }

        public static Matrix3x2 Multiply(Matrix3x2 value1, Matrix3x2 value2) { throw null; }

        public static Matrix3x2 Multiply(Matrix3x2 value1, float value2) { throw null; }

        public static Matrix3x2 Negate(Matrix3x2 value) { throw null; }

        public static Matrix3x2 operator +(Matrix3x2 value1, Matrix3x2 value2) { throw null; }

        public static bool operator ==(Matrix3x2 value1, Matrix3x2 value2) { throw null; }

        public static bool operator !=(Matrix3x2 value1, Matrix3x2 value2) { throw null; }

        public static Matrix3x2 operator *(Matrix3x2 value1, Matrix3x2 value2) { throw null; }

        public static Matrix3x2 operator *(Matrix3x2 value1, float value2) { throw null; }

        public static Matrix3x2 operator -(Matrix3x2 value1, Matrix3x2 value2) { throw null; }

        public static Matrix3x2 operator -(Matrix3x2 value) { throw null; }

        public static Matrix3x2 Subtract(Matrix3x2 value1, Matrix3x2 value2) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        public float M11;
        public float M12;
        public float M13;
        public float M14;
        public float M21;
        public float M22;
        public float M23;
        public float M24;
        public float M31;
        public float M32;
        public float M33;
        public float M34;
        public float M41;
        public float M42;
        public float M43;
        public float M44;
        public Matrix4x4(Matrix3x2 value) { }

        public Matrix4x4(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44) { }

        public static Matrix4x4 Identity { get { throw null; } }

        public bool IsIdentity { get { throw null; } }

        public Vector3 Translation { get { throw null; } set { } }

        public static Matrix4x4 Add(Matrix4x4 value1, Matrix4x4 value2) { throw null; }

        public static Matrix4x4 CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3 cameraForwardVector) { throw null; }

        public static Matrix4x4 CreateConstrainedBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3 cameraForwardVector, Vector3 objectForwardVector) { throw null; }

        public static Matrix4x4 CreateFromAxisAngle(Vector3 axis, float angle) { throw null; }

        public static Matrix4x4 CreateFromQuaternion(Quaternion quaternion) { throw null; }

        public static Matrix4x4 CreateFromYawPitchRoll(float yaw, float pitch, float roll) { throw null; }

        public static Matrix4x4 CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector) { throw null; }

        public static Matrix4x4 CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane) { throw null; }

        public static Matrix4x4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane) { throw null; }

        public static Matrix4x4 CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance) { throw null; }

        public static Matrix4x4 CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance) { throw null; }

        public static Matrix4x4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance) { throw null; }

        public static Matrix4x4 CreateReflection(Plane value) { throw null; }

        public static Matrix4x4 CreateRotationX(float radians, Vector3 centerPoint) { throw null; }

        public static Matrix4x4 CreateRotationX(float radians) { throw null; }

        public static Matrix4x4 CreateRotationY(float radians, Vector3 centerPoint) { throw null; }

        public static Matrix4x4 CreateRotationY(float radians) { throw null; }

        public static Matrix4x4 CreateRotationZ(float radians, Vector3 centerPoint) { throw null; }

        public static Matrix4x4 CreateRotationZ(float radians) { throw null; }

        public static Matrix4x4 CreateScale(Vector3 scales, Vector3 centerPoint) { throw null; }

        public static Matrix4x4 CreateScale(Vector3 scales) { throw null; }

        public static Matrix4x4 CreateScale(float scale, Vector3 centerPoint) { throw null; }

        public static Matrix4x4 CreateScale(float xScale, float yScale, float zScale, Vector3 centerPoint) { throw null; }

        public static Matrix4x4 CreateScale(float xScale, float yScale, float zScale) { throw null; }

        public static Matrix4x4 CreateScale(float scale) { throw null; }

        public static Matrix4x4 CreateShadow(Vector3 lightDirection, Plane plane) { throw null; }

        public static Matrix4x4 CreateTranslation(Vector3 position) { throw null; }

        public static Matrix4x4 CreateTranslation(float xPosition, float yPosition, float zPosition) { throw null; }

        public static Matrix4x4 CreateWorld(Vector3 position, Vector3 forward, Vector3 up) { throw null; }

        public static bool Decompose(Matrix4x4 matrix, out Vector3 scale, out Quaternion rotation, out Vector3 translation) { throw null; }

        public bool Equals(Matrix4x4 other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public float GetDeterminant() { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool Invert(Matrix4x4 matrix, out Matrix4x4 result) { throw null; }

        public static Matrix4x4 Lerp(Matrix4x4 matrix1, Matrix4x4 matrix2, float amount) { throw null; }

        public static Matrix4x4 Multiply(Matrix4x4 value1, Matrix4x4 value2) { throw null; }

        public static Matrix4x4 Multiply(Matrix4x4 value1, float value2) { throw null; }

        public static Matrix4x4 Negate(Matrix4x4 value) { throw null; }

        public static Matrix4x4 operator +(Matrix4x4 value1, Matrix4x4 value2) { throw null; }

        public static bool operator ==(Matrix4x4 value1, Matrix4x4 value2) { throw null; }

        public static bool operator !=(Matrix4x4 value1, Matrix4x4 value2) { throw null; }

        public static Matrix4x4 operator *(Matrix4x4 value1, Matrix4x4 value2) { throw null; }

        public static Matrix4x4 operator *(Matrix4x4 value1, float value2) { throw null; }

        public static Matrix4x4 operator -(Matrix4x4 value1, Matrix4x4 value2) { throw null; }

        public static Matrix4x4 operator -(Matrix4x4 value) { throw null; }

        public static Matrix4x4 Subtract(Matrix4x4 value1, Matrix4x4 value2) { throw null; }

        public override string ToString() { throw null; }

        public static Matrix4x4 Transform(Matrix4x4 value, Quaternion rotation) { throw null; }

        public static Matrix4x4 Transpose(Matrix4x4 matrix) { throw null; }
    }

    public partial struct Plane : IEquatable<Plane>
    {
        public float D;
        public Vector3 Normal;
        public Plane(Vector3 normal, float d) { }

        public Plane(Vector4 value) { }

        public Plane(float x, float y, float z, float d) { }

        public static Plane CreateFromVertices(Vector3 point1, Vector3 point2, Vector3 point3) { throw null; }

        public static float Dot(Plane plane, Vector4 value) { throw null; }

        public static float DotCoordinate(Plane plane, Vector3 value) { throw null; }

        public static float DotNormal(Plane plane, Vector3 value) { throw null; }

        public bool Equals(Plane other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static Plane Normalize(Plane value) { throw null; }

        public static bool operator ==(Plane value1, Plane value2) { throw null; }

        public static bool operator !=(Plane value1, Plane value2) { throw null; }

        public override string ToString() { throw null; }

        public static Plane Transform(Plane plane, Matrix4x4 matrix) { throw null; }

        public static Plane Transform(Plane plane, Quaternion rotation) { throw null; }
    }

    public partial struct Quaternion : IEquatable<Quaternion>
    {
        public float W;
        public float X;
        public float Y;
        public float Z;
        public Quaternion(Vector3 vectorPart, float scalarPart) { }

        public Quaternion(float x, float y, float z, float w) { }

        public static Quaternion Identity { get { throw null; } }

        public bool IsIdentity { get { throw null; } }

        public static Quaternion Add(Quaternion value1, Quaternion value2) { throw null; }

        public static Quaternion Concatenate(Quaternion value1, Quaternion value2) { throw null; }

        public static Quaternion Conjugate(Quaternion value) { throw null; }

        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle) { throw null; }

        public static Quaternion CreateFromRotationMatrix(Matrix4x4 matrix) { throw null; }

        public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll) { throw null; }

        public static Quaternion Divide(Quaternion value1, Quaternion value2) { throw null; }

        public static float Dot(Quaternion quaternion1, Quaternion quaternion2) { throw null; }

        public bool Equals(Quaternion other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static Quaternion Inverse(Quaternion value) { throw null; }

        public float Length() { throw null; }

        public float LengthSquared() { throw null; }

        public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, float amount) { throw null; }

        public static Quaternion Multiply(Quaternion value1, Quaternion value2) { throw null; }

        public static Quaternion Multiply(Quaternion value1, float value2) { throw null; }

        public static Quaternion Negate(Quaternion value) { throw null; }

        public static Quaternion Normalize(Quaternion value) { throw null; }

        public static Quaternion operator +(Quaternion value1, Quaternion value2) { throw null; }

        public static Quaternion operator /(Quaternion value1, Quaternion value2) { throw null; }

        public static bool operator ==(Quaternion value1, Quaternion value2) { throw null; }

        public static bool operator !=(Quaternion value1, Quaternion value2) { throw null; }

        public static Quaternion operator *(Quaternion value1, Quaternion value2) { throw null; }

        public static Quaternion operator *(Quaternion value1, float value2) { throw null; }

        public static Quaternion operator -(Quaternion value1, Quaternion value2) { throw null; }

        public static Quaternion operator -(Quaternion value) { throw null; }

        public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount) { throw null; }

        public static Quaternion Subtract(Quaternion value1, Quaternion value2) { throw null; }

        public override string ToString() { throw null; }
    }

    public static partial class Vector
    {
        public static bool IsHardwareAccelerated { get { throw null; } }

        public static Vector<T> Abs<T>(Vector<T> value)
            where T : struct { throw null; }

        public static Vector<T> Add<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<T> AndNot<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<byte> AsVectorByte<T>(Vector<T> value)
            where T : struct { throw null; }

        public static Vector<double> AsVectorDouble<T>(Vector<T> value)
            where T : struct { throw null; }

        public static Vector<short> AsVectorInt16<T>(Vector<T> value)
            where T : struct { throw null; }

        public static Vector<int> AsVectorInt32<T>(Vector<T> value)
            where T : struct { throw null; }

        public static Vector<long> AsVectorInt64<T>(Vector<T> value)
            where T : struct { throw null; }

        [CLSCompliant(false)]
        public static Vector<sbyte> AsVectorSByte<T>(Vector<T> value)
            where T : struct { throw null; }

        public static Vector<float> AsVectorSingle<T>(Vector<T> value)
            where T : struct { throw null; }

        [CLSCompliant(false)]
        public static Vector<ushort> AsVectorUInt16<T>(Vector<T> value)
            where T : struct { throw null; }

        [CLSCompliant(false)]
        public static Vector<uint> AsVectorUInt32<T>(Vector<T> value)
            where T : struct { throw null; }

        [CLSCompliant(false)]
        public static Vector<ulong> AsVectorUInt64<T>(Vector<T> value)
            where T : struct { throw null; }

        public static Vector<T> BitwiseAnd<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<T> BitwiseOr<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<float> ConditionalSelect(Vector<int> condition, Vector<float> left, Vector<float> right) { throw null; }

        public static Vector<double> ConditionalSelect(Vector<long> condition, Vector<double> left, Vector<double> right) { throw null; }

        public static Vector<T> ConditionalSelect<T>(Vector<T> condition, Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<double> ConvertToDouble(Vector<long> value) { throw null; }

        [CLSCompliant(false)]
        public static Vector<double> ConvertToDouble(Vector<ulong> value) { throw null; }

        public static Vector<int> ConvertToInt32(Vector<float> value) { throw null; }

        public static Vector<long> ConvertToInt64(Vector<double> value) { throw null; }

        public static Vector<float> ConvertToSingle(Vector<int> value) { throw null; }

        [CLSCompliant(false)]
        public static Vector<float> ConvertToSingle(Vector<uint> value) { throw null; }

        [CLSCompliant(false)]
        public static Vector<uint> ConvertToUInt32(Vector<float> value) { throw null; }

        [CLSCompliant(false)]
        public static Vector<ulong> ConvertToUInt64(Vector<double> value) { throw null; }

        public static Vector<T> Divide<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static T Dot<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<long> Equals(Vector<double> left, Vector<double> right) { throw null; }

        public static Vector<int> Equals(Vector<int> left, Vector<int> right) { throw null; }

        public static Vector<long> Equals(Vector<long> left, Vector<long> right) { throw null; }

        public static Vector<int> Equals(Vector<float> left, Vector<float> right) { throw null; }

        public static Vector<T> Equals<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static bool EqualsAll<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static bool EqualsAny<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<long> GreaterThan(Vector<double> left, Vector<double> right) { throw null; }

        public static Vector<int> GreaterThan(Vector<int> left, Vector<int> right) { throw null; }

        public static Vector<long> GreaterThan(Vector<long> left, Vector<long> right) { throw null; }

        public static Vector<int> GreaterThan(Vector<float> left, Vector<float> right) { throw null; }

        public static Vector<T> GreaterThan<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static bool GreaterThanAll<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static bool GreaterThanAny<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<long> GreaterThanOrEqual(Vector<double> left, Vector<double> right) { throw null; }

        public static Vector<int> GreaterThanOrEqual(Vector<int> left, Vector<int> right) { throw null; }

        public static Vector<long> GreaterThanOrEqual(Vector<long> left, Vector<long> right) { throw null; }

        public static Vector<int> GreaterThanOrEqual(Vector<float> left, Vector<float> right) { throw null; }

        public static Vector<T> GreaterThanOrEqual<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static bool GreaterThanOrEqualAll<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static bool GreaterThanOrEqualAny<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<long> LessThan(Vector<double> left, Vector<double> right) { throw null; }

        public static Vector<int> LessThan(Vector<int> left, Vector<int> right) { throw null; }

        public static Vector<long> LessThan(Vector<long> left, Vector<long> right) { throw null; }

        public static Vector<int> LessThan(Vector<float> left, Vector<float> right) { throw null; }

        public static Vector<T> LessThan<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static bool LessThanAll<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static bool LessThanAny<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<long> LessThanOrEqual(Vector<double> left, Vector<double> right) { throw null; }

        public static Vector<int> LessThanOrEqual(Vector<int> left, Vector<int> right) { throw null; }

        public static Vector<long> LessThanOrEqual(Vector<long> left, Vector<long> right) { throw null; }

        public static Vector<int> LessThanOrEqual(Vector<float> left, Vector<float> right) { throw null; }

        public static Vector<T> LessThanOrEqual<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static bool LessThanOrEqualAll<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static bool LessThanOrEqualAny<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<T> Max<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<T> Min<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<T> Multiply<T>(T left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<T> Multiply<T>(Vector<T> left, T right)
            where T : struct { throw null; }

        public static Vector<T> Multiply<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        public static Vector<float> Narrow(Vector<double> low, Vector<double> high) { throw null; }

        [CLSCompliant(false)]
        public static Vector<sbyte> Narrow(Vector<short> low, Vector<short> high) { throw null; }

        public static Vector<short> Narrow(Vector<int> low, Vector<int> high) { throw null; }

        public static Vector<int> Narrow(Vector<long> low, Vector<long> high) { throw null; }

        [CLSCompliant(false)]
        public static Vector<byte> Narrow(Vector<ushort> low, Vector<ushort> high) { throw null; }

        [CLSCompliant(false)]
        public static Vector<ushort> Narrow(Vector<uint> low, Vector<uint> high) { throw null; }

        [CLSCompliant(false)]
        public static Vector<uint> Narrow(Vector<ulong> low, Vector<ulong> high) { throw null; }

        public static Vector<T> Negate<T>(Vector<T> value)
            where T : struct { throw null; }

        public static Vector<T> OnesComplement<T>(Vector<T> value)
            where T : struct { throw null; }

        public static Vector<T> SquareRoot<T>(Vector<T> value)
            where T : struct { throw null; }

        public static Vector<T> Subtract<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }

        [CLSCompliant(false)]
        public static void Widen(Vector<byte> source, out Vector<ushort> low, out Vector<ushort> high) { throw null; }

        public static void Widen(Vector<short> source, out Vector<int> low, out Vector<int> high) { throw null; }

        public static void Widen(Vector<int> source, out Vector<long> low, out Vector<long> high) { throw null; }

        [CLSCompliant(false)]
        public static void Widen(Vector<sbyte> source, out Vector<short> low, out Vector<short> high) { throw null; }

        public static void Widen(Vector<float> source, out Vector<double> low, out Vector<double> high) { throw null; }

        [CLSCompliant(false)]
        public static void Widen(Vector<ushort> source, out Vector<uint> low, out Vector<uint> high) { throw null; }

        [CLSCompliant(false)]
        public static void Widen(Vector<uint> source, out Vector<ulong> low, out Vector<ulong> high) { throw null; }

        public static Vector<T> Xor<T>(Vector<T> left, Vector<T> right)
            where T : struct { throw null; }
    }

    public partial struct Vector2 : IEquatable<Vector2>, IFormattable
    {
        public float X;
        public float Y;
        public Vector2(float x, float y) { }

        public Vector2(float value) { }

        public static Vector2 One { get { throw null; } }

        public static Vector2 UnitX { get { throw null; } }

        public static Vector2 UnitY { get { throw null; } }

        public static Vector2 Zero { get { throw null; } }

        public static Vector2 Abs(Vector2 value) { throw null; }

        public static Vector2 Add(Vector2 left, Vector2 right) { throw null; }

        public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max) { throw null; }

        public void CopyTo(float[] array, int index) { }

        public void CopyTo(float[] array) { }

        public static float Distance(Vector2 value1, Vector2 value2) { throw null; }

        public static float DistanceSquared(Vector2 value1, Vector2 value2) { throw null; }

        public static Vector2 Divide(Vector2 left, Vector2 right) { throw null; }

        public static Vector2 Divide(Vector2 left, float divisor) { throw null; }

        public static float Dot(Vector2 value1, Vector2 value2) { throw null; }

        public bool Equals(Vector2 other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public float Length() { throw null; }

        public float LengthSquared() { throw null; }

        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount) { throw null; }

        public static Vector2 Max(Vector2 value1, Vector2 value2) { throw null; }

        public static Vector2 Min(Vector2 value1, Vector2 value2) { throw null; }

        public static Vector2 Multiply(Vector2 left, Vector2 right) { throw null; }

        public static Vector2 Multiply(Vector2 left, float right) { throw null; }

        public static Vector2 Multiply(float left, Vector2 right) { throw null; }

        public static Vector2 Negate(Vector2 value) { throw null; }

        public static Vector2 Normalize(Vector2 value) { throw null; }

        public static Vector2 operator +(Vector2 left, Vector2 right) { throw null; }

        public static Vector2 operator /(Vector2 left, Vector2 right) { throw null; }

        public static Vector2 operator /(Vector2 value1, float value2) { throw null; }

        public static bool operator ==(Vector2 left, Vector2 right) { throw null; }

        public static bool operator !=(Vector2 left, Vector2 right) { throw null; }

        public static Vector2 operator *(Vector2 left, Vector2 right) { throw null; }

        public static Vector2 operator *(Vector2 left, float right) { throw null; }

        public static Vector2 operator *(float left, Vector2 right) { throw null; }

        public static Vector2 operator -(Vector2 left, Vector2 right) { throw null; }

        public static Vector2 operator -(Vector2 value) { throw null; }

        public static Vector2 Reflect(Vector2 vector, Vector2 normal) { throw null; }

        public static Vector2 SquareRoot(Vector2 value) { throw null; }

        public static Vector2 Subtract(Vector2 left, Vector2 right) { throw null; }

        public override string ToString() { throw null; }

        public string ToString(string format, IFormatProvider formatProvider) { throw null; }

        public string ToString(string format) { throw null; }

        public static Vector2 Transform(Vector2 position, Matrix3x2 matrix) { throw null; }

        public static Vector2 Transform(Vector2 position, Matrix4x4 matrix) { throw null; }

        public static Vector2 Transform(Vector2 value, Quaternion rotation) { throw null; }

        public static Vector2 TransformNormal(Vector2 normal, Matrix3x2 matrix) { throw null; }

        public static Vector2 TransformNormal(Vector2 normal, Matrix4x4 matrix) { throw null; }
    }

    public partial struct Vector3 : IEquatable<Vector3>, IFormattable
    {
        public float X;
        public float Y;
        public float Z;
        public Vector3(Vector2 value, float z) { }

        public Vector3(float x, float y, float z) { }

        public Vector3(float value) { }

        public static Vector3 One { get { throw null; } }

        public static Vector3 UnitX { get { throw null; } }

        public static Vector3 UnitY { get { throw null; } }

        public static Vector3 UnitZ { get { throw null; } }

        public static Vector3 Zero { get { throw null; } }

        public static Vector3 Abs(Vector3 value) { throw null; }

        public static Vector3 Add(Vector3 left, Vector3 right) { throw null; }

        public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max) { throw null; }

        public void CopyTo(float[] array, int index) { }

        public void CopyTo(float[] array) { }

        public static Vector3 Cross(Vector3 vector1, Vector3 vector2) { throw null; }

        public static float Distance(Vector3 value1, Vector3 value2) { throw null; }

        public static float DistanceSquared(Vector3 value1, Vector3 value2) { throw null; }

        public static Vector3 Divide(Vector3 left, Vector3 right) { throw null; }

        public static Vector3 Divide(Vector3 left, float divisor) { throw null; }

        public static float Dot(Vector3 vector1, Vector3 vector2) { throw null; }

        public bool Equals(Vector3 other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public float Length() { throw null; }

        public float LengthSquared() { throw null; }

        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount) { throw null; }

        public static Vector3 Max(Vector3 value1, Vector3 value2) { throw null; }

        public static Vector3 Min(Vector3 value1, Vector3 value2) { throw null; }

        public static Vector3 Multiply(Vector3 left, Vector3 right) { throw null; }

        public static Vector3 Multiply(Vector3 left, float right) { throw null; }

        public static Vector3 Multiply(float left, Vector3 right) { throw null; }

        public static Vector3 Negate(Vector3 value) { throw null; }

        public static Vector3 Normalize(Vector3 value) { throw null; }

        public static Vector3 operator +(Vector3 left, Vector3 right) { throw null; }

        public static Vector3 operator /(Vector3 left, Vector3 right) { throw null; }

        public static Vector3 operator /(Vector3 value1, float value2) { throw null; }

        public static bool operator ==(Vector3 left, Vector3 right) { throw null; }

        public static bool operator !=(Vector3 left, Vector3 right) { throw null; }

        public static Vector3 operator *(Vector3 left, Vector3 right) { throw null; }

        public static Vector3 operator *(Vector3 left, float right) { throw null; }

        public static Vector3 operator *(float left, Vector3 right) { throw null; }

        public static Vector3 operator -(Vector3 left, Vector3 right) { throw null; }

        public static Vector3 operator -(Vector3 value) { throw null; }

        public static Vector3 Reflect(Vector3 vector, Vector3 normal) { throw null; }

        public static Vector3 SquareRoot(Vector3 value) { throw null; }

        public static Vector3 Subtract(Vector3 left, Vector3 right) { throw null; }

        public override string ToString() { throw null; }

        public string ToString(string format, IFormatProvider formatProvider) { throw null; }

        public string ToString(string format) { throw null; }

        public static Vector3 Transform(Vector3 position, Matrix4x4 matrix) { throw null; }

        public static Vector3 Transform(Vector3 value, Quaternion rotation) { throw null; }

        public static Vector3 TransformNormal(Vector3 normal, Matrix4x4 matrix) { throw null; }
    }

    public partial struct Vector4 : IEquatable<Vector4>, IFormattable
    {
        public float W;
        public float X;
        public float Y;
        public float Z;
        public Vector4(Vector2 value, float z, float w) { }

        public Vector4(Vector3 value, float w) { }

        public Vector4(float x, float y, float z, float w) { }

        public Vector4(float value) { }

        public static Vector4 One { get { throw null; } }

        public static Vector4 UnitW { get { throw null; } }

        public static Vector4 UnitX { get { throw null; } }

        public static Vector4 UnitY { get { throw null; } }

        public static Vector4 UnitZ { get { throw null; } }

        public static Vector4 Zero { get { throw null; } }

        public static Vector4 Abs(Vector4 value) { throw null; }

        public static Vector4 Add(Vector4 left, Vector4 right) { throw null; }

        public static Vector4 Clamp(Vector4 value1, Vector4 min, Vector4 max) { throw null; }

        public void CopyTo(float[] array, int index) { }

        public void CopyTo(float[] array) { }

        public static float Distance(Vector4 value1, Vector4 value2) { throw null; }

        public static float DistanceSquared(Vector4 value1, Vector4 value2) { throw null; }

        public static Vector4 Divide(Vector4 left, Vector4 right) { throw null; }

        public static Vector4 Divide(Vector4 left, float divisor) { throw null; }

        public static float Dot(Vector4 vector1, Vector4 vector2) { throw null; }

        public bool Equals(Vector4 other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public float Length() { throw null; }

        public float LengthSquared() { throw null; }

        public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amount) { throw null; }

        public static Vector4 Max(Vector4 value1, Vector4 value2) { throw null; }

        public static Vector4 Min(Vector4 value1, Vector4 value2) { throw null; }

        public static Vector4 Multiply(Vector4 left, Vector4 right) { throw null; }

        public static Vector4 Multiply(Vector4 left, float right) { throw null; }

        public static Vector4 Multiply(float left, Vector4 right) { throw null; }

        public static Vector4 Negate(Vector4 value) { throw null; }

        public static Vector4 Normalize(Vector4 vector) { throw null; }

        public static Vector4 operator +(Vector4 left, Vector4 right) { throw null; }

        public static Vector4 operator /(Vector4 left, Vector4 right) { throw null; }

        public static Vector4 operator /(Vector4 value1, float value2) { throw null; }

        public static bool operator ==(Vector4 left, Vector4 right) { throw null; }

        public static bool operator !=(Vector4 left, Vector4 right) { throw null; }

        public static Vector4 operator *(Vector4 left, Vector4 right) { throw null; }

        public static Vector4 operator *(Vector4 left, float right) { throw null; }

        public static Vector4 operator *(float left, Vector4 right) { throw null; }

        public static Vector4 operator -(Vector4 left, Vector4 right) { throw null; }

        public static Vector4 operator -(Vector4 value) { throw null; }

        public static Vector4 SquareRoot(Vector4 value) { throw null; }

        public static Vector4 Subtract(Vector4 left, Vector4 right) { throw null; }

        public override string ToString() { throw null; }

        public string ToString(string format, IFormatProvider formatProvider) { throw null; }

        public string ToString(string format) { throw null; }

        public static Vector4 Transform(Vector2 position, Matrix4x4 matrix) { throw null; }

        public static Vector4 Transform(Vector2 value, Quaternion rotation) { throw null; }

        public static Vector4 Transform(Vector3 position, Matrix4x4 matrix) { throw null; }

        public static Vector4 Transform(Vector3 value, Quaternion rotation) { throw null; }

        public static Vector4 Transform(Vector4 vector, Matrix4x4 matrix) { throw null; }

        public static Vector4 Transform(Vector4 value, Quaternion rotation) { throw null; }
    }

    public partial struct Vector<T> : IEquatable<Vector<T>>, IFormattable where T : struct
    {
        private int _dummyPrimitive;
        public Vector(T value) { }

        public Vector(T[] values, int index) { }

        public Vector(T[] values) { }

        public static int Count { get { throw null; } }

        public T this[int index] { get { throw null; } }

        public static Vector<T> One { get { throw null; } }

        public static Vector<T> Zero { get { throw null; } }

        public void CopyTo(T[] destination, int startIndex) { }

        public void CopyTo(T[] destination) { }

        public bool Equals(Vector<T> other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static Vector<T> operator +(Vector<T> left, Vector<T> right) { throw null; }

        public static Vector<T> operator &(Vector<T> left, Vector<T> right) { throw null; }

        public static Vector<T> operator |(Vector<T> left, Vector<T> right) { throw null; }

        public static Vector<T> operator /(Vector<T> left, Vector<T> right) { throw null; }

        public static bool operator ==(Vector<T> left, Vector<T> right) { throw null; }

        public static Vector<T> operator ^(Vector<T> left, Vector<T> right) { throw null; }

        public static explicit operator Vector<byte>(Vector<T> value) { throw null; }

        public static explicit operator Vector<double>(Vector<T> value) { throw null; }

        public static explicit operator Vector<short>(Vector<T> value) { throw null; }

        public static explicit operator Vector<int>(Vector<T> value) { throw null; }

        public static explicit operator Vector<long>(Vector<T> value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator Vector<sbyte>(Vector<T> value) { throw null; }

        public static explicit operator Vector<float>(Vector<T> value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator Vector<ushort>(Vector<T> value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator Vector<uint>(Vector<T> value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator Vector<ulong>(Vector<T> value) { throw null; }

        public static bool operator !=(Vector<T> left, Vector<T> right) { throw null; }

        public static Vector<T> operator *(T factor, Vector<T> value) { throw null; }

        public static Vector<T> operator *(Vector<T> value, T factor) { throw null; }

        public static Vector<T> operator *(Vector<T> left, Vector<T> right) { throw null; }

        public static Vector<T> operator ~(Vector<T> value) { throw null; }

        public static Vector<T> operator -(Vector<T> left, Vector<T> right) { throw null; }

        public static Vector<T> operator -(Vector<T> value) { throw null; }

        public override string ToString() { throw null; }

        public string ToString(string format, IFormatProvider formatProvider) { throw null; }

        public string ToString(string format) { throw null; }
    }
}