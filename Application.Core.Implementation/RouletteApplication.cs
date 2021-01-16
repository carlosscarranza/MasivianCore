using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Domain.Core.Classes;
using Domain.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Utilities.Core.Interfaces.Database.Repositories;
using Utilities.Core.Interfaces.Database.UnitOfWorks;

namespace Application.Core.Implementation
{
    public class RouletteApplication : IRouletteApplication
    {
        private readonly IRepositoryAsync<Roulette> _rouletteRepositoryAsync;
        private readonly IRepositoryAsync<Bet> _betRepositoryAsync;
        private readonly IRepositoryAsync<User> _useRepositoryAsync;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public RouletteApplication(IRepositoryAsync<Roulette> rouletteRepositoryAsync, 
            IUnitOfWorkAsync unitOfWorkAsync, IRepositoryAsync<Bet> betRepositoryAsync, IRepositoryAsync<User> useRepositoryAsync)
        {
            _rouletteRepositoryAsync = rouletteRepositoryAsync;
            _unitOfWorkAsync = unitOfWorkAsync;
            _betRepositoryAsync = betRepositoryAsync;
            _useRepositoryAsync = useRepositoryAsync;
        }

        public async Task<Guid> CreateRoulette(Roulette input)
        {
            //Setup
            input.State = false;

            await _rouletteRepositoryAsync.InsertAsync(input);
            await _unitOfWorkAsync.SaveChangesAsync();

            return input.Id;
        }

        public async Task<bool> OpenRoulette(Guid input)
        {
            var existsRoulette = await _rouletteRepositoryAsync.GetAll().FirstOrDefaultAsync(x => x.Id == input);

            if (existsRoulette == null) return false;

            //Setup
            existsRoulette.State = true;
            existsRoulette.DateOpening = DateTime.UtcNow.ToString("O");

            await _rouletteRepositoryAsync.UpdateAsync(existsRoulette);
            await _unitOfWorkAsync.SaveChangesAsync();

            return true;
        }

        public async Task<string> CloseRoulette(Guid input)
        {
            var existsRoulette = await _rouletteRepositoryAsync.GetAll().FirstOrDefaultAsync(x => x.Id == input);

            if (existsRoulette == null) throw new Exception("The roulette doesn't exists");

            existsRoulette.State = false;
            existsRoulette.DateClosing = DateTime.UtcNow.ToString("O");
            await _rouletteRepositoryAsync.UpdateAsync(existsRoulette);
            await _unitOfWorkAsync.SaveChangesAsync();

            switch (existsRoulette.Type)
            {
                case RouletteType.Colour:
                {
                    var colours = Enum.GetValues(typeof(ColourType)).Cast<string>().ToList(); ;
                    Random rand = new Random();
                    int index = rand.Next(colours.Count);

                    var winnerBet = await _betRepositoryAsync.GetAll()
                        .Include(x => x.User).FirstOrDefaultAsync(x => x.ColourBet == (ColourType) index);

                    var amountWinner = await _betRepositoryAsync.GetAll()
                        .Where(x => x.CreationTime >= DateTime.Parse(existsRoulette.DateOpening) && x.CreationTime <= DateTime.Parse(existsRoulette.DateClosing) && x.RouletteId == existsRoulette.Id)
                        .SumAsync(x => x.Amount * 1.8);
                
                    return $"The winner is {winnerBet.User.Name} with a prize of {amountWinner}.";
                }
                case RouletteType.Number:
                {
                    Random rand = new Random();
                    int numberWinner = rand.Next(0, 36);

                    var winnerBet = await _betRepositoryAsync.GetAll()
                        .Include(x => x.User).FirstOrDefaultAsync(x => x.NumberBet == numberWinner);

                    var amountWinner = await _betRepositoryAsync.GetAll()
                        .Where(x => x.CreationTime >= DateTime.Parse(existsRoulette.DateOpening) && x.CreationTime <= DateTime.Parse(existsRoulette.DateClosing) && x.RouletteId == winnerBet.RouletteId)
                        .SumAsync(x => x.Amount * 5);

                    return $"The winner is {winnerBet.User.Name} with a prize of {amountWinner}.";
                }
                default:
                    return $"There isn't winner";
            }
        }

        public async Task<List<Roulette>> GetRoulettes()
        {
            return await _rouletteRepositoryAsync.GetAll().ToListAsync();
        }
    }
}
