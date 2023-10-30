public class Program
{
    private static readonly int NumberOfTasks = 5;
    private static readonly int NumberOfIterations = 10_000;
    private static volatile int Sum = 0;

    // Just a generic object used for locking
    private static readonly object oLock = new object();
    private static void DoCalculation()
    {
        for (int i = 0; i < NumberOfIterations; i++)
        {
            lock (oLock)
            {
                // Only one thread can take lock and
                // execute this block of code
                Sum++;
                // The lock is automatically released
                // when the thread exits this block of code
            }
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