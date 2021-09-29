using System;
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
        private readonly OffersService _offersService;

        public OffersController()
        {
            _offersService = new OffersService();
        }

        [HttpGet] // "api/offers"
        public List<Offer> Get()
        {
            // should return a list of all user-posted offers
            return _offersService.GetAll();
        }

        [HttpGet("{id:int}")] // "api/offers/<number>"
        public Offer Get(int id)
        {
            // should return an offer having a specified id
            // the id will likely be index in a static list
            // until a database is created
            return _offersService.GetById(id);
        }
        
        [HttpPost("add")]// "api/offers/add"
        public void PostOffer(Offer offer)
        {
            _offersService.Save(offer);
            /*Console.WriteLine("------");
            foreach (Offer offer1 in _offersService.GetAll())
            {
                Console.WriteLine(offer1.ToString());
            }*/
        }
    }
}