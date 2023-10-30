class Program
{
    private static readonly object oLock1 = new object();
    private static readonly object oLock2 = new object();
    private static void DoWork1()
    {
        lock (oLock1)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} took lock 1");
            Thread.Sleep(1000);
            lock (oLock2)
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} took lock 2");
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} doing work...");
                Thread.Sleep(1000);
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} finished work!");
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} releasing lock 2");
            }
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} released lock 1");
        }
    }

    private static void DoWork2()
    {
        lock (oLock2)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} took lock 2");
            Thread.Sleep(1000);
            lock (oLock1)
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} took lock 1");
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} doing work...");
                Thread.Sleep(1000);
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} finished work!");
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} releasing lock 1");
            }
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} released lock 2");
        }
    }

    public static async Task Main()
    {
        var tasks = new Task[]
        {
            Task.Run(DoWork1),
            Task.Run(DoWork2)
        };

        await Task.WhenAll(tasks);

    }
}