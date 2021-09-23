using System.Collections.Generic;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // "api/offers"
    public class OffersController : ControllerBase
    {
        public OffersController() {}

        [HttpGet] // "api/offers"
        public List<Offer> Get()
        {
            // should return a list of all user-posted offers
            return OffersService.GetAll();
        }

        [HttpGet("{id:int}")] // "api/offers/<number>"
        public Offer Get(int id)
        {
            // should return an offer having a specified id
            // the id will likely be index in a static list
            // until a database is created
            return OffersService.GetById(id);
        }
    }
}