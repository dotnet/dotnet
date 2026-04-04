// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Resources.Container.Tests;

internal class TempFile : IDisposable
{
    public TempFile()
    {
        this.FilePath = Path.GetTempFileName();
    }

    public string FilePath { get; set; }

    public void Write(string data)
    {
        using (var stream = new FileStream(this.FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete))
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(data);
            }
        }
    }

    public void Dispose()
    {
        for (var tries = 0; ; tries++)
        {
            try
            {
                File.Delete(this.FilePath);
                return;
            }
            catch (IOException) when (tries < 3)
            {
                // the file is unavailable because it is: still being written to or being processed by another thread
                // sleep for sometime before deleting
                Thread.Sleep(1000);
            }
        }
    }
}
