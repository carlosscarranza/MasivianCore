using System;
using System.Threading.Tasks;
using Domain.Core;

namespace Application.Core.Interfaces
{
    public interface IRouletteApplication
    {
        Task<Guid> CreateRoulette(Roulette input);
    }
}
