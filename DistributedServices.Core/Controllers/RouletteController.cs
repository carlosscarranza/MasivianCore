﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Domain.Core.Classes;

namespace DistributedServices.Core.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly IRouletteApplication _rouletteApplication;

        public RouletteController(IRouletteApplication rouletteApplication)
        {
            _rouletteApplication = rouletteApplication;
        }

        [HttpPost("v1/roulette")]
        public Task<Guid> CreateRoulette([FromBody] Roulette input)
        {
            return _rouletteApplication.CreateRoulette(input);
        }

        [HttpGet("v1/roulette")]
        public Task<List<Roulette>> GetRoulettes()
        {
            return _rouletteApplication.GetRoulettes();
        }

        [HttpPut("v1/open-roulette")]
        public Task<bool> OpenRoulette([FromForm][Required] Guid input)
        {
            return _rouletteApplication.OpenRoulette(input);
        }

        [HttpPut("v1/close-roulette")]
        public Task<string> CloseRoulette([FromForm][Required] Guid input)
        {
            return _rouletteApplication.CloseRoulette(input);
        }
    }
}
