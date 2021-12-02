using System.Linq;
using backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SummaryController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("cities")]
        public object Cities()
        {
            var offersByCity =
                _db.Offers
                    .GroupBy(o => o.Address.City)
                    .Select(offers => new {City = offers.Key, Count = offers.Count()}).ToList();
            
            return offersByCity;
        }
    }
}