using System;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Domain.Core;
using Utilities.Core.Interfaces.Database.Repositories;
using Utilities.Core.Interfaces.Database.UnitOfWorks;

namespace Application.Core.Implementation
{
    public class RouletteApplication : IRouletteApplication
    {
        private readonly IRepositoryAsync<Roulette> _rouletteRepositoryAsync;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public RouletteApplication(IRepositoryAsync<Roulette> rouletteRepositoryAsync, IUnitOfWorkAsync unitOfWorkAsync)
        {
            _rouletteRepositoryAsync = rouletteRepositoryAsync;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public async Task<Guid> CreateRoulette(Roulette input)
        {
            await _rouletteRepositoryAsync.InsertAsync(input);
            await _unitOfWorkAsync.SaveChangesAsync();

            return input.Id;
        }
    }
}
