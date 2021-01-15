using System;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Domain.Core;
using Domain.Core.Classes;
using Microsoft.EntityFrameworkCore;
using Utilities.Core.Interfaces.Database.Repositories;
using Utilities.Core.Interfaces.Database.UnitOfWorks;

namespace Application.Core.Implementation
{
    public class BetApplication : IBetApplication
    {
        private readonly IRepositoryAsync<Bet> _betRepositoryAsync;
        private readonly IRepositoryAsync<Roulette> _rouletteRepositoryAsync;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public BetApplication(IRepositoryAsync<Bet> betRepositoryAsync, IUnitOfWorkAsync unitOfWorkAsync, IRepositoryAsync<Roulette> rouletteRepositoryAsync)
        {
            _betRepositoryAsync = betRepositoryAsync;
            _unitOfWorkAsync = unitOfWorkAsync;
            _rouletteRepositoryAsync = rouletteRepositoryAsync;
        }

        public async Task<Guid> CreateBet(Bet input)
        {
            var isActiveRoulette = await _rouletteRepositoryAsync.GetAll().AnyAsync(x => x.State == true && x.Id == input.RouletteId);

            if (!isActiveRoulette) throw new Exception("The roulette doesn't exists");

            await _betRepositoryAsync.InsertAsync(input);
            await _unitOfWorkAsync.SaveChangesAsync();

            return input.Id;

        }
    }
}
