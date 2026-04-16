namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Metrics;
    using static System.FormattableString;

    internal class CommonHelper
    {
        public static void TrackValueHelper(Metric metricToTrack, double metricValue, string[] dimValues)
        {
            int numberOfDimensions = dimValues.Length;
            switch (numberOfDimensions)
            {
                case 1:
                    {
                        metricToTrack.TrackValue(metricValue, dimValues[0]);
                        break;
                    }

                case 2:
                    {
                        metricToTrack.TrackValue(metricValue,
                            dimValues[0],
                            dimValues[1]);
                        break;
                    }

                case 3:
                    {
                        metricToTrack.TrackValue(metricValue,
                            dimValues[0],
                            dimValues[1],
                            dimValues[2]);
                        break;
                    }

                case 4:
                    {
                        metricToTrack.TrackValue(metricValue,
                            dimValues[0],
                            dimValues[1],
                            dimValues[2],
                            dimValues[3]);
                        break;
                    }

                case 5:
                    {
                        metricToTrack.TrackValue(metricValue,
                            dimValues[0],
                            dimValues[1],
                            dimValues[2],
                            dimValues[3],
                            dimValues[4]);
                        break;
                    }

                case 6:
                    {
                        metricToTrack.TrackValue(metricValue,
                            dimValues[0],
                            dimValues[1],
                            dimValues[2],
                            dimValues[3],
                            dimValues[4],
                            dimValues[5]);
                        break;
                    }

                case 7:
                    {
                        metricToTrack.TrackValue(metricValue,
                             dimValues[0],
                             dimValues[1],
                             dimValues[2],
                             dimValues[3],
                             dimValues[4],
                             dimValues[5],
                             dimValues[6]);
                        break;
                    }

                case 8:
                    {
                        metricToTrack.TrackValue(metricValue,
                             dimValues[0],
                             dimValues[1],
                             dimValues[2],
                             dimValues[3],
                             dimValues[4],
                             dimValues[5],
                             dimValues[6],
                             dimValues[7]);
                        break;
                    }

                case 9:
                    {
                        metricToTrack.TrackValue(metricValue,
                            dimValues[0],
                            dimValues[1],
                            dimValues[2],
                            dimValues[3],
                            dimValues[4],
                            dimValues[5],
                            dimValues[6],
                            dimValues[7],
                            dimValues[8]);
                        break;
                    }

                case 10:
                    {
                        metricToTrack.TrackValue(metricValue,
                            dimValues[0],
                            dimValues[1],
                            dimValues[2],
                            dimValues[3],
                            dimValues[4],
                            dimValues[5],
                            dimValues[6],
                            dimValues[7],
                            dimValues[8],
                            dimValues[9]);
                        break;
                    }

                default:
                    {
                        //// This should be caught and properly logged by the base class:
                        throw new InvalidOperationException(Invariant($"Cannot track metric value.")
                                                          + Invariant($" There number of dimensions {numberOfDimensions} is more than the supported limit of 10."));
                    }
            }
        }
    }
}