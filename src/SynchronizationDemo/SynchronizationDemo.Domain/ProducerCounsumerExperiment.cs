public class ProducerConsumerExperiment
{
    
    private const int BufferSize = 4;
    private const int NProducers = 2;
    private const int NConsumers = 2;

    public void Run()
    {
        //qui serve una struttura dati per posizionare i prodotti creati e consumati
        Queue<int> buffer = new ();


        var producersSemaphore = new SemaphoreSlim(BufferSize, BufferSize);
        var elementiDisponibili = new SemaphoreSlim(0, BufferSize);
        var accessoTavolo = new SemaphoreSlim(1, 1);

        Thread[] produttori = new Thread[NProducers];
        Thread[] consumatori = new Thread[NConsumers];

        //TODO: implemenazione dei metodi per produrre e per consumare
        void ConsumeSomeProduct(int consumerId)
        {
   
        }

        void ProduceSomeProduct(int producerId)
        {
            while (true)
            {
                int foodItem = new Random().Next(100);

                spaziDisponibili.Wait();
                try 
                {
                    accessoTavolo.Wait();
                    try
                    {
                        buffer.Enqueue(foodItem);
                        Console.WriteLine($"Produttore {producerId} ha prodotto l'elemento {foodItem}");
                    }
                    finally
                    {
                        accessoTavolo.Release();
                    }
                }
                catch
                {
                    elementiDisponibili.Release();
                }

                elementiDisponibili.Release();
            }
        }


        for (int i = 1; i < NConsumers; i++)
        {
            consumatori[i] = new Thread(() => ConsumerSomeProduct(i));
            consumatori[i].Start();
            
        }


         for (int i = 1; i < NProducers; i++)
        {
            produttori[i] = new Thread(() => MakeSomeProduct(i));
            produttori[i].Start();
            
        }

        foreach (Thread consumatore in consumatori)
        {
            consumatore.Join();
        }

        foreach (Thread produttore in produttori)
        {
            produttore.Join();
        }

    }
      
}