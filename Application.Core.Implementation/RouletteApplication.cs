using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Domain.Core;
using Domain.Core.Classes;
using Microsoft.EntityFrameworkCore;
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
            //Setup
            input.State = false;

            await _rouletteRepositoryAsync.InsertAsync(input);
            await _unitOfWorkAsync.SaveChangesAsync();

            return input.Id;
        }

        public async Task<bool> OpenRoulette(Roulette input)
        {
            var existsRoulette = await _rouletteRepositoryAsync.GetAll().FirstOrDefaultAsync(x => x.Id == input.Id);

            if (existsRoulette == null) return false;

            //Setup
            existsRoulette.State = true;
            existsRoulette.DateOpening = DateTime.UtcNow.ToString("O");

            await _rouletteRepositoryAsync.UpdateAsync(existsRoulette);
            await _unitOfWorkAsync.SaveChangesAsync();

            return true;
        }

        public async Task<List<Roulette>> GetRoulettes()
        {
            return await _rouletteRepositoryAsync.GetAll().ToListAsync();
        }
    }
}
