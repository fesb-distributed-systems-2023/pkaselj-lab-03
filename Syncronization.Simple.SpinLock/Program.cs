public class Program
{
    private static readonly int NumberOfTasks = 5;
    private static readonly int NumberOfIterations = 10_000;
    private static volatile int Sum = 0;

    // 0 indicates free, 1 indicates locked
    private static int IsFree = 0;
    private static void DoCalculation()
    {
        for (int i = 0; i < NumberOfIterations; i++)
        {
            // Try to take a lock (set IsFree to 1), or sleep for 1000ms
            // if the lock is already taken
            while(Interlocked.Exchange(ref IsFree, 1) == 1)
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} tried to obtain the lock. The lock is already taken.");
                Task.Delay(1000);
            }

            // Perform the operation uninterruptedly
            Sum++;

            // Release the lock (atomically!!!)
            _ = Interlocked.Exchange(ref IsFree, 0);
        }
    }
    public static async Task Main()
    {
        List<Task> tasks = new List<Task>();
        for (int i = 0; i < NumberOfTasks; i++)
        {
            var task = Task.Run(DoCalculation);
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);

        Console.WriteLine($"Sum = {Sum}");
    }
}