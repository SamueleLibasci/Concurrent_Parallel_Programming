public class SemaphoreExperiment
{
    public void Run() {
        var stampante = new SemaphoreSlim(1, 1);

        Thread[] lavoratori = new Thread[4];
        int idLavoratore = 0;

        for (int i = 0; i < lavoratori.Length; i++)
        {

            idLavoratore = i+1;
            lavoratori[i] = new Thread(() =>
            {
                Console.WriteLine($"ID: {idLavoratore}, vuole accedere alla risorsa condivisa");
                stampante.Wait();
                Console.WriteLine($"ID: {idLavoratore}, ottiene la risorsa condivisa");
                Thread.Sleep(1500); // Simula il tempo di utilizzo della risorsa condivisa
                Console.WriteLine($"ID: {idLavoratore}, rilascio la risorsa condivisa");

                stampante.Release();
            });
        }

        foreach(Thread lavoratore in lavoratori)
        {
            lavoratore.Start();   
        }

        foreach (Thread lavoratore in lavoratori)
        {
            lavoratore.Join();
        }
    
        Console.WriteLine("Tutti i lavoratori hanno terminato l'accesso alla stampante");
    }
}