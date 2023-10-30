public class Program
{
    private static readonly int NumberOfTasks = 10;
    private static readonly int NumberOfIterations = 10;

    private static readonly Random random = new Random();

    // Number of threads allowed to execute some block of code at the same time
    private const int NumberOfThreadsAllowed = 3;
    // Semaphore - A lock that allows multiple (`NumberOfThreadsAllowed`) at the same time, but no more!s
    private static Semaphore Semaphore = new Semaphore(NumberOfThreadsAllowed, NumberOfThreadsAllowed);

    private static void DoCalculation()
    {
        for (int i = 0; i < NumberOfIterations; i++)
        {
            // Take a lock (decrement semaphore by 1)
            Semaphore.WaitOne();

            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} took a lock!");
            int workDuration_ms = 1000 + random.Next(2000);
            Thread.Sleep(workDuration_ms);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} released a lock!");

            // Release the lock (increment semaphore by 1)
            Semaphore.Release();
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
    }
}