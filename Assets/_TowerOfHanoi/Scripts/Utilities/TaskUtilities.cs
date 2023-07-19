using System.Threading;

namespace TowerOfHanoi.Utilities
{
    public static class TaskUtilities
    {
        public static void CancelTasks(CancellationTokenSource[] sources)
        {
            foreach (var source in sources)
            {
                source.Cancel();
            }
        }

        public static void DisposeCancellationSources(CancellationTokenSource[] sources)
        {
            foreach (var source in sources)
            {
                source.Dispose();
            }
        }
    }
}