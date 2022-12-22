using System;
using System.Threading.Tasks;

namespace Microsoft.Arcade.Common
{
    public interface IHelpers
    {
        string ComputeSha256Hash(string normalizedPath);

        T MutexExec<T>(Func<T> function, string mutexName);
        T MutexExec<T>(Func<Task<T>> function, string mutexName);
        void MutexExec(Func<Task> function, string mutexName);

        T DirectoryMutexExec<T>(Func<T> function, string path);
        T DirectoryMutexExec<T>(Func<Task<T>> function, string path);
        void DirectoryMutexExec(Func<Task> function, string path);

        string RemoveTrailingSlash(string directoryPath);
    }
}
