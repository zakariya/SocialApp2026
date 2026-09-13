using API.Data;
using API.Entities;
using API.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : BaseApiController
    {
        private readonly AppDbContext _db;
        private readonly CorsCacheService _cache;

        public SubscriptionController(AppDbContext db, CorsCacheService cache)
        {
            _db = db;
            _cache = cache;
        }

        // Endpoint to refresh a client’s origin after payment
        [HttpPost("refresh/{clientId}")]
        public IActionResult RefreshClient(string clientId)
        {
            var client = _db.Aggregators.FirstOrDefault(c => c.ClientId == clientId);

            if (client == null)
                return NotFound("Client not found");

            if (client.SubscriptionExpiry <= DateTime.UtcNow)
            {
                _cache.RemoveOrigin(client.Url);
                return BadRequest("Subscription expired");
            }
            if (!client.IsActive)
            {
                _cache.RemoveOrigin(client.Url);
                return BadRequest($"Client is inactive. Origin {client.Url} removed from allowed list");
            }


            _cache.AddOrigin(client.Url);

            return Ok($"Origin {client.Url} added to allowed list");
        }
    }
}