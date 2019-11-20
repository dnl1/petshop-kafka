using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using PetShop.Domain;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PetShop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(Pet pet)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "kafka:29092"
            };

            var producer = new ProducerBuilder<Null, string>(config).Build();

            using (producer)
            {
                try
                {
                    var result = await producer.ProduceAsync("new-pet", new Message<Null, string> { Value = JsonSerializer.Serialize(pet) });

                    return Ok(result);
                }
                catch (ProduceException<Null, string> ex)
                {
                    return StatusCode(500, ex);
                }
            }
        }
    }
}