using System.Diagnostics;
using ThreadDemo.Domain;

var experiments = new ThreadExperiments();

while (true)
{
    Console.Clear();
    Console.WriteLine("=== THREAD DEMO ===");
    Console.WriteLine("1. Esecuzione sequenziale");
    Console.WriteLine("2. Due thread concorrenti (con pause diverse)");
    Console.WriteLine("3. Proprietà del thread corrente");
    Console.WriteLine("4. Contatore senza lock");
    Console.WriteLine("5. Contatore con lock");
    Console.WriteLine("6. Background worker senza Join");
    Console.WriteLine("0. Esci");
    Console.Write("\nScelta: ");

    string? choice = Console.ReadLine()?.Trim();

    switch (choice)
    {
        case "1":
            experiments.RunSequential();
            break;

        case "2":
            experiments.RunThreads();
            break;

        case "3":
            experiments.ShowThreadProperties();
            break;

        case "4":
        case "5":
            bool useLock = choice == "5";
            var sw = Stopwatch.StartNew();
            var result = experiments.RunCounter(useLock);
            sw.Stop();

            Console.WriteLine($"\nModalità: {(useLock ? "CON lock" : "SENZA lock")}");
            Console.WriteLine($"Atteso:   {result.Expected:N0}");
            Console.WriteLine($"Ottenuto: {result.Actual:N0}");
            Console.WriteLine($"Persi:    {result.Expected - result.Actual:N0}");
            Console.WriteLine($"Tempo:    {sw.ElapsedMilliseconds} ms");
            break;

        case "6":
            experiments.RunBackgroundWorker();
            break;

        case "0":
        case null:
            return;

        default:
            Console.WriteLine("Scelta non valida.");
            break;
    }

    Console.WriteLine("\nPremi INVIO per continuare...");
    Console.ReadLine();
}
