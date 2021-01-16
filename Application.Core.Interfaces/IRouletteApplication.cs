using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.Classes;

namespace Application.Core.Interfaces
{
    public interface IRouletteApplication
    {
        Task<Guid> CreateRoulette(Roulette input);
        
        Task<bool> OpenRoulette(Guid input);

        Task<string> CloseRoulette(Guid input);

        Task<List<Roulette>> GetRoulettes();
    }
}
