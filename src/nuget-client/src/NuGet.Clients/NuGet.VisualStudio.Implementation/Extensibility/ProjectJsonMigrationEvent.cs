using System;
using System.Globalization;
using NuGet.Common;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    internal class ProjectJsonMigrationEvent : TelemetryEvent
    {
        private const string ProjectJsonMigrationEventName = "ProjectJsonMigration";

        internal ProjectJsonMigrationEvent(
            string projectId,
            string projectFilePath,
            NuGetOperationStatus status,
            DateTimeOffset startTime,
            double duration) :
            base(ProjectJsonMigrationEventName)
        {
            if (projectId == null) throw new ArgumentNullException(nameof(projectId));
            if (projectFilePath == null) throw new ArgumentNullException(nameof(projectFilePath));

            base[nameof(ProjectId)] = projectId;
            AddPiiData(nameof(ProjectFilePath), projectFilePath);
            base[nameof(Status)] = status;
            base[nameof(StartTime)] = startTime.UtcDateTime.ToString("O", CultureInfo.InvariantCulture);
            base[nameof(EndTime)] = DateTimeOffset.Now.UtcDateTime.ToString("O", CultureInfo.InvariantCulture);
            base[nameof(Duration)] = duration;
        }

        internal string ProjectId => (string)base[nameof(ProjectId)];
        internal string ProjectFilePath => (string)base[nameof(ProjectFilePath)];
        internal NuGetOperationStatus Status => (NuGetOperationStatus)base[nameof(NuGetOperationStatus)];
        internal string StartTime => (string)base[nameof(StartTime)];
        internal string EndTime => (string)base[nameof(EndTime)];
        internal double Duration => (double)base[nameof(Duration)];
    }
}
