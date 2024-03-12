using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
#pragma warning disable CA1822  // Method should be static
    using Microsoft.ApplicationInsights.Metrics.TestUtility;

    /// <summary />
    [TestClass]
    public class MultidimensionalCube2Tests
    {
        /// <summary />
        [TestMethod]
        public void Ctors()
        {
            int[] dimensionValuesCountLimits = new int[] { 10, 11, 12, 13 };
            int factoryCallsCount = 0;
            object[] lastFactoryCall = null;

            {
                var cube = new MultidimensionalCube2<int>(
                                        (vector) => { Interlocked.Increment(ref factoryCallsCount); lastFactoryCall = vector; return 18; },
                                        (int []) dimensionValuesCountLimits);

                CtorTestImplementation(cube, ref dimensionValuesCountLimits, ref factoryCallsCount, ref lastFactoryCall, "2", 18, Int32.MaxValue);
            }
            {
                var cube = new MultidimensionalCube2<int>(
                                        3,
                                        (vector) => { Interlocked.Increment(ref factoryCallsCount); lastFactoryCall = vector; return -6; },
                                        (int []) dimensionValuesCountLimits);

                CtorTestImplementation(cube, ref dimensionValuesCountLimits, ref factoryCallsCount, ref lastFactoryCall, "4", -6, 3);
            }
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_PropagesUserException()
        {
            var cube = new MultidimensionalCube2<int>(
                                        (vector) => { if (vector[0].Equals("magic")) return 42; else throw new InvalidOperationException("User Exception"); },
                                        10, 11, 12);

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.ThrowsException<InvalidOperationException>( () => cube.TryGetOrCreatePoint("A", "B", "C") );
            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.ThrowsException<ArgumentNullException>( () => cube.TryGetOrCreatePoint("A", null, "C") );
            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.AreEqual(false, cube.TryGetPoint("magic", "B", "C").IsSuccess);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, cube.TryGetPoint("magic", "B", "C").ResultCode);
            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.AreEqual(42, cube.TryGetOrCreatePoint("magic", "B", "C").Point);
            Assert.AreEqual(42, cube.TryGetPoint("magic", "B", "C").Point);

            Assert.AreEqual(1, cube.TotalPointsCount);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_IncorrectDimsDetected()
        {
            var cube = new MultidimensionalCube2<int>(
                                        (vector) => 0,
                                        10, 11, 12);

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.ThrowsException<ArgumentException>(() => cube.TryGetOrCreatePoint("A", "B"));
            Assert.ThrowsException<ArgumentException>(() => cube.TryGetPoint("A", "B"));

            Assert.ThrowsException<ArgumentException>(() => cube.TryGetOrCreatePoint("A", "B", "C", "D"));
            Assert.ThrowsException<ArgumentException>(() => cube.TryGetPoint("A", "B", "C", "D"));

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.IsTrue(cube.TryGetOrCreatePoint("A", "B", "C1").IsSuccess);
            Assert.IsTrue(cube.TryGetPoint("A", "B", "C1").IsSuccess);
            Assert.IsFalse(cube.TryGetPoint("A", "B", "C2").IsSuccess);

            Assert.AreEqual(1, cube.TotalPointsCount);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_NullParameters()
        {
            var cube = new MultidimensionalCube2<int>(
                                        (vector) => 0,
                                        10, 11, 12);

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetOrCreatePoint("A", null, "B"));
            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetPoint(null, "B", "B"));
            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetPoint("A", null, "B"));

            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetOrCreatePoint(null));
            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetPoint(null));

            Assert.AreEqual(0, cube.TotalPointsCount);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_RespectsDimensionValuesCountLimits()
        {
            int[] dimensionValuesCountLimits = new int[] { 3, 5, 4 };


            var cube = new MultidimensionalCube2<int>(
                                        (vector) => { return Int32.Parse(vector[0]); },
                                        dimensionValuesCountLimits);

            MultidimensionalPointResult<int> result;
            for (int d1 = 0; d1 < dimensionValuesCountLimits[0]; d1++)
            {
                for (int d2 = 0; d2 < dimensionValuesCountLimits[1]; d2++)
                {
                    for (int d3 = 0; d3 < dimensionValuesCountLimits[2]; d3++)
                    {
                        result = cube.TryGetPoint($"{d1 * 10}", $"{d2}", $"{d3}");
                        Assert.IsFalse(result.IsSuccess);
                        Assert.AreEqual(0, result.Point);
                        Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
                        Assert.AreEqual(
                                    d3 + d2 * dimensionValuesCountLimits[2] + d1 * dimensionValuesCountLimits[1] * dimensionValuesCountLimits[2],
                                    cube.TotalPointsCount,
                                    $"d3 = {d3}; d2 = {d2}; d1 = {d1}");

                        result = cube.TryGetOrCreatePoint($"{d1 * 10}", $"{d2}", $"{d3}");
                        Assert.IsTrue(result.IsSuccess);
                        Assert.AreEqual(d1 * 10, result.Point);
                        Assert.AreEqual(
                                    1 + d3 + d2 * dimensionValuesCountLimits[2] + d1 * dimensionValuesCountLimits[1] * dimensionValuesCountLimits[2],
                                    cube.TotalPointsCount,
                                    $"d3 = {d3}; d2 = {d2}; d1 = {d1}");

                        result = cube.TryGetPoint($"{d1 * 10}", $"{d2}", $"{d3}");
                        Assert.IsTrue(result.IsSuccess);
                        Assert.AreEqual(d1 * 10, result.Point);
                        Assert.AreEqual(
                                    1 + d3 + d2 * dimensionValuesCountLimits[2] + d1 * dimensionValuesCountLimits[1] * dimensionValuesCountLimits[2],
                                    cube.TotalPointsCount,
                                    $"d3 = {d3}; d2 = {d2}; d1 = {d1}");
                    }

                    result = cube.TryGetOrCreatePoint($"{d1 * 10}", $"{d2}", $"{dimensionValuesCountLimits[2]}");
                    Assert.IsFalse(result.IsSuccess);
                    Assert.AreEqual(0, result.Point);
                    Assert.AreEqual(result.ResultCode, MultidimensionalPointResultCodes.Failure_SubdimensionsCountLimitReached);
                    Assert.AreEqual(2, result.FailureCoordinateIndex);

                    result = cube.TryGetPoint($"{d1 * 10}", $"{d2}", $"{dimensionValuesCountLimits[2]}");
                    Assert.IsFalse(result.IsSuccess);
                    Assert.AreEqual(0, result.Point);
                    Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
                    Assert.AreEqual(-1, result.FailureCoordinateIndex);

                }

                result = cube.TryGetOrCreatePoint($"{d1 * 10}", $"{dimensionValuesCountLimits[1]}", $"{dimensionValuesCountLimits[2]}");
                Assert.IsFalse(result.IsSuccess);
                Assert.AreEqual(0, result.Point);
                Assert.AreEqual(result.ResultCode, MultidimensionalPointResultCodes.Failure_SubdimensionsCountLimitReached);
                Assert.AreEqual(1, result.FailureCoordinateIndex);

                result = cube.TryGetPoint($"{d1 * 10}", $"{dimensionValuesCountLimits[1]}", $"{dimensionValuesCountLimits[2]}");
                Assert.IsFalse(result.IsSuccess);
                Assert.AreEqual(0, result.Point);
                Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
                Assert.AreEqual(-1, result.FailureCoordinateIndex);
            }

            result = cube.TryGetOrCreatePoint($"{dimensionValuesCountLimits[0] * 10}", $"{dimensionValuesCountLimits[1]}", $"{dimensionValuesCountLimits[2]}");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(result.ResultCode, MultidimensionalPointResultCodes.Failure_SubdimensionsCountLimitReached);
            Assert.AreEqual(0, result.FailureCoordinateIndex);

            result = cube.TryGetPoint($"{dimensionValuesCountLimits[0] * 10}", $"{dimensionValuesCountLimits[1]}", $"{dimensionValuesCountLimits[2]}");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_RespectsTotalPointsCountLimit()
        {
            string[] lastFactoryCall = null;

            var cube = new MultidimensionalCube2<int>(
                                        1000,
                                        (vector) => { lastFactoryCall = vector;  return Int32.Parse(vector[0]); },
                                        10000, 10000, 10000);

            Assert.AreEqual(1000, cube.TotalPointsCountLimit);

            MultidimensionalPointResult<int> result;
            for (int p = 0; p < 1000; p++)
            {
                result = cube.TryGetPoint($"{p}", "foo", "bar");
                Assert.IsFalse(result.IsSuccess);
                Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
                Assert.AreEqual(0, result.Point);
                Assert.AreEqual(p, cube.TotalPointsCount);

                TestUtil.AssertAreEqual(p == 0 ? null : new string[] { $"{p - 1}", "foo", "bar" }, lastFactoryCall);

                result = cube.TryGetOrCreatePoint($"{p}", "foo", "bar");
                Assert.IsTrue(result.IsSuccess);
                Assert.IsTrue(result.IsPointCreatedNew);
                Assert.AreEqual(p, result.Point);
                Assert.AreEqual(p + 1, cube.TotalPointsCount);

                TestUtil.AssertAreEqual(new string[] { $"{p}", "foo", "bar" }, lastFactoryCall);

                result = cube.TryGetPoint($"{p}", "foo", "bar");
                Assert.IsTrue(result.IsSuccess);
                Assert.IsFalse(result.IsPointCreatedNew);
                Assert.AreEqual(p, result.Point);
                Assert.AreEqual(p + 1, cube.TotalPointsCount);

                TestUtil.AssertAreEqual(new string[] { $"{p}", "foo", "bar" }, lastFactoryCall);
            }

            result = cube.TryGetOrCreatePoint($"{1000}", "foo", "bar"); 
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(result.ResultCode, MultidimensionalPointResultCodes.Failure_TotalPointsCountLimitReached);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(1000, cube.TotalPointsCount);
            TestUtil.AssertAreEqual(new string[] { $"999", "foo", "bar" }, lastFactoryCall);

            result = cube.TryGetPoint($"{1000}", "foo", "bar");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(1000, cube.TotalPointsCount);
            TestUtil.AssertAreEqual(new string[] { $"999", "foo", "bar" }, lastFactoryCall);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_GetsAndCreates()
        {
            var cube = new MultidimensionalCube2<int>(
                                        1000,
                                        (vector) => Int32.Parse(vector[0]),
                                        10000, 10000, 10000);

            MultidimensionalPointResult<int> result;

            Assert.AreEqual(0, cube.TotalPointsCount);

            result = cube.TryGetPoint("1", "1", "1");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(0, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetPoint("1", "1", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);


            result = cube.TryGetPoint("1", "1", "7");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetPoint("1", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);


            result = cube.TryGetPoint("2", "1", "7");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("2", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(2, result.Point);
            Assert.AreEqual(3, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("2", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(2, result.Point);
            Assert.AreEqual(3, cube.TotalPointsCount);

            result = cube.TryGetPoint("2", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(2, result.Point);
            Assert.AreEqual(3, cube.TotalPointsCount);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_GetsAndCreatesWithDimCap()
        {
            string dimCapValue = "999";

            var cube = new MultidimensionalCube2<int>(
                                        8,
                                        (vector) => Int32.Parse(vector[0]), true,
                                        dimCapValue,
                                        2, 2, 2);

            MultidimensionalPointResult<int> result;

            Assert.AreEqual(0, cube.TotalPointsCount);

            result = cube.TryGetPoint("1", "1", "1");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(0, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetPoint("1", "1", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);


            result = cube.TryGetPoint("1", "1", "2");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "2");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "2");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetPoint("1", "1", "3");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            // Triggers creation of fallback dimension value.
            // i.e 3 will be replaced with "dimCapValue"
            result = cube.TryGetOrCreatePoint("1", "1", "3");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreatedAboveDimCapLimit, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(3, cube.TotalPointsCount);

            result = cube.TryGetPoint("1", "1", "3");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(3, cube.TotalPointsCount);

            // Triggers re-use of previously created fallback dimension value.            
            result = cube.TryGetOrCreatePoint("1", "1", "3");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(3, cube.TotalPointsCount);

            // Force 2nd dimension to reach cap. (preparing for the next case below)
            result = cube.TryGetOrCreatePoint("1", "2", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(4, cube.TotalPointsCount);


            // Triggers creation of fallback dimension value.
            // i.e both 3 will be replaced with "dimCapValue"
            result = cube.TryGetOrCreatePoint("1", "3", "3");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreatedAboveDimCapLimit, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(5, cube.TotalPointsCount);

            // Force 1st dimension to reach cap. (preparing for the next case below)
            result = cube.TryGetOrCreatePoint("2", "2", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(2, result.Point);
            Assert.AreEqual(6, cube.TotalPointsCount);


            // Triggers creation of fallback dimension value.
            // i.e all 3 will be replaced with "dimCapValue"
            result = cube.TryGetOrCreatePoint("3", "3", "3");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreatedAboveDimCapLimit, result.ResultCode);
            Assert.AreEqual(999, result.Point);
            Assert.AreEqual(7, cube.TotalPointsCount);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_GetsAndCreatesWithDimCapReachingOverallCap()
        {
            string dimCapValue = "999";

            var cube = new MultidimensionalCube2<int>(
                                        4,
                                        (vector) => Int32.Parse(vector[0]), true,
                                        dimCapValue,
                                        2, 2, 2);

            MultidimensionalPointResult<int> result;

            Assert.AreEqual(0, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "2", "1");
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "2");
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(3, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("2", "1", "1");
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(4, cube.TotalPointsCount);

            // At this stage 4 series have been created, and it means overall metric cap is hit.
            // The following will respect that limit, and will not attempt to do dim capping for the 3rd dimension.
            result = cube.TryGetOrCreatePoint("1", "1", "3");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(4, cube.TotalPointsCount);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_TotalPointsCountLimitReached, result.ResultCode);
        }

        /// <summary />
        [TestMethod]
        public void GetAllPoints()
        {
            var cube = new MultidimensionalCube2<int>(
                                        1000,
                                        (vector) => Int32.Parse(vector[0]),
                                        10000, 10000, 10000);

            cube.TryGetOrCreatePoint("1", "2", "3");
            cube.TryGetOrCreatePoint("10", "20", "30");
            cube.TryGetOrCreatePoint("45", "12", "-8");
            cube.TryGetOrCreatePoint("10", "20", "30");
            cube.TryGetOrCreatePoint("1", "5", "3");
            cube.TryGetOrCreatePoint("1", "2", "4");
            cube.TryGetOrCreatePoint("-6", "14", "132");

            Assert.AreEqual(6, cube.TotalPointsCount);

            IReadOnlyCollection<KeyValuePair<string[], int>> allPoints1 = cube.GetAllPoints();

            var allPoints2 = new Dictionary<string[], int>(new ArrayEqualityComparer<string[]>());
            cube.GetAllPoints(allPoints2);

            Assert.AreEqual(cube.TotalPointsCount, allPoints1.Count);
            Assert.AreEqual(cube.TotalPointsCount, allPoints2.Count);
           
            Action<IReadOnlyCollection<KeyValuePair<string[], int>>, IDictionary<string[], int>, string[], int> AssertContains =
                    (points1, points2, expectedVector, expectedPoint) =>
                    {
                        Assert.AreEqual(1, points1.Where((p) => TestUtil.AreEqual(expectedVector, p.Key)).Count());
                        Assert.AreEqual(expectedPoint, points1.First( (p) => TestUtil.AreEqual(expectedVector, p.Key) ).Value);

                        Assert.IsTrue(points2.ContainsKey(expectedVector));
                        Assert.AreEqual(expectedPoint, points2[expectedVector]);
                    };

            AssertContains(allPoints1, allPoints2, new string[] { "1", "2", "3" }, 1);
            AssertContains(allPoints1, allPoints2, new string[] { "10", "20", "30" }, 10);
            AssertContains(allPoints1, allPoints2, new string[] { "45", "12", "-8" }, 45);
            AssertContains(allPoints1, allPoints2, new string[] { "1", "5", "3" }, 1);
            AssertContains(allPoints1, allPoints2, new string[] { "1", "2", "4" }, 1);
            AssertContains(allPoints1, allPoints2, new string[] { "-6", "14", "132" }, -6);
        }

        /// <summary />
        [TestMethod]
        public async Task TryGetOrCreatePointAsync()
        {
            var cube = new MultidimensionalCube2<int>(
                                        1000,
                                        (vector) => Int32.Parse(vector[0]),
                                        10000, 10000, 10000);

            MultidimensionalPointResult<int> result;

            Assert.AreEqual(0, cube.TotalPointsCount);

            result = await cube.TryGetOrCreatePointAsync("10", "20", "30");
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(true, result.IsPointCreatedNew);
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual(10, result.Point);

            result = await cube.TryGetOrCreatePointAsync("10", "20", "30");
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(false, result.IsPointCreatedNew);
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual(10, result.Point);
        }

        private void CtorTestImplementation(
                            MultidimensionalCube2<int> cube, ref int[] dimensionValuesCountLimits,
                            ref int factoryCallsCount,
                            ref object[] lastFactoryCall,
                            string suffix,
                            int point,
                            int totalPointsCountLimit)
        {
            Assert.IsNotNull(cube);

            Assert.AreEqual(dimensionValuesCountLimits.Length, cube.DimensionsCount);

            for (int d = 0; d < dimensionValuesCountLimits.Length; d++)
            {
                int limit = dimensionValuesCountLimits[d];
                Assert.AreEqual(limit, cube.GetDimensionValuesCountLimit(d));
            }

            Assert.AreEqual(totalPointsCountLimit, cube.TotalPointsCountLimit);

            string[] newPointVector = new string[] { $"A{suffix}", $"B{suffix}", $"C{suffix}", $"D{suffix}" };
            MultidimensionalPointResult<int> result = cube.TryGetOrCreatePoint(newPointVector);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(point, result.Point);

            TestUtil.AssertAreEqual(newPointVector, lastFactoryCall);
        }
    }
#pragma warning restore CA1822  // Method should be static
}
