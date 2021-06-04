using System;
using System.Threading.Tasks;

namespace IdentityTranslationLite.Core
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Note: improved implementation @ https://devblogs.microsoft.com/pfxteam/crafting-a-task-timeoutafter-method/ ??
        /// </summary>
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
        {
            if (task == await Task.WhenAny(task, Task.Delay(timeout)))
                return await task;
            else
                throw new TimeoutException();
        }
    }
}
