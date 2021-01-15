using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Domain.Core.Classes;

namespace DistributedServices.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IBetApplication _betApplication;

        public BetController(IBetApplication betApplication)
        {
            _betApplication = betApplication;
        }

        [HttpPost("v1/bet")]
        public Task<Guid> CreateBet([FromBody] Bet input, [FromHeader][Required] Guid userId)
        {
            //Setup
            input.UserId = userId;

            return _betApplication.CreateBet(input);
        }
    }
}
