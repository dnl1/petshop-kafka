using Confluent.Kafka;
using System;
using System.Threading;

namespace PetShop.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe("new-pet");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            Console.WriteLine("Connected to Kafka");

            try
            {
                while (true)
                {
                    try
                    {
                        var cr = consumer.Consume(cts.Token);

                        Console.WriteLine("Got Pet {0}", cr.Value);
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine("Error {0}", ex.Message);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }
}