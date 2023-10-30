public class Program
{
    private const int NumberOfTasks = 8;
    private const int NumberOfIterations = 10_000;
    private static volatile int Sum = 0;
    private static void DoCalculation()
    {
        for (int i = 0; i < NumberOfIterations; i++)
        {
            // Performe sum atomically i.e. "in one go"
            // Nothing can interrupt this sum in the middle of operation
            Interlocked.Increment(ref Sum);
        }
    }
    public static async Task Main()
    {
        List<Task> tasks = new List<Task>();
        for (int i = 0;i < NumberOfTasks;i++)
        {
            var task = Task.Run(DoCalculation);
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);

        Console.WriteLine($"Sum = {Sum}");
    }
}