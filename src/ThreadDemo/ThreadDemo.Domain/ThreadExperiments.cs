namespace ThreadDemo.Domain;

public class ThreadExperiments
{
    public static int counter = 0;
    public static readonly object gateLock = new();

    public void RunSequential()
    {
        Work("A");
        Work("B");
    }

    public void RunThreads()
    {
        // A aspetta 300 ms a ciclo (più veloce), B aspetta 800 ms a ciclo (più lento)
        Thread thread1 = new Thread(() => Work("A", 300));
        Thread thread2 = new Thread(() => Work("B", 800));
        Console.WriteLine("Starting threads....");

        thread1.Start();
        thread2.Start();

        // Pausa nel thread principale: 1 secondo (1000 ms)
        Thread.Sleep(1000);
        Console.WriteLine(">>> [Main] Sono passato dopo 1s: A e B stanno ancora lavorando!");

        thread1.Join(); // Il Main aspetta la fine di A
        Console.WriteLine(">>> [Main] A ha finito! Ora aspetto che finisca B...");

        thread2.Join(); // Il Main aspetta la fine di B (il più lento)
        Console.WriteLine("All finished");
    }

    public void ShowThreadProperties()
    {
        Thread currentThread = Thread.CurrentThread;
        Console.WriteLine($"Thread ID: {currentThread.ManagedThreadId}");
        Console.WriteLine($"Thread Name: {currentThread.Name}");
        Console.WriteLine($"Is Background: {currentThread.IsBackground}");
        Console.WriteLine($"Is Alive: {currentThread.IsAlive}");
        Console.WriteLine($"Priority: {currentThread.Priority}");
        Console.WriteLine($"Is a Thread pool Thread: {currentThread.IsThreadPoolThread}");
    }

    private static void Increment(int iterations, bool useLock)
    {
        for (int i = 0; i < iterations; i++)
        {
            if (useLock)
            {
                lock (gateLock)
                {
                    counter++;
                }
            }
            else
            {
                counter++;
            }
        }
    }

    // 1° Test: i thread contano sulla stessa variabile condivisa senza sincronizzazione
    // 2° Test: i thread contano sulla stessa variabile condivisa con sincronizzazione
    public (int Expected, int Actual) RunCounter(bool useLock, int iterations = 100_000)
    {
        counter = 0;

        Thread t1 = new(() => Increment(iterations, useLock));
        Thread t2 = new(() => Increment(iterations, useLock));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        return (Expected: iterations * 2, Actual: counter);
    }

    // Avvia un thread di background senza Join
    public void RunBackgroundWorker()
    {
        Thread worker = new Thread(() =>
        {
            Console.WriteLine("\n[Background Worker] Avviato! Attendo 5 secondi...");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            Console.WriteLine("\n[Background Worker] Finito! (Se hai premuto subito 0, non mi vedrai mai)");
        });

        worker.IsBackground = true;
        worker.Start();

        Console.WriteLine("[Main] Worker avviato senza Join. Premi INVIO per tornare al menu e scegli subito 0.");
    }

    // Funzione per simulare il lavoro con durata configurabile
    private void Work(string name, int delayMs = 100)
    {
        for (int i = 0; i <= 5; i++)
        {
            Console.WriteLine($"{name}: Thread: {Thread.CurrentThread.ManagedThreadId}; index {i}");
            Thread.Sleep(delayMs);
        }
    }
}

