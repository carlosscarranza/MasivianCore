using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.Classes;

namespace Application.Core.Interfaces
{
    public interface IBetApplication
    {
        Task<Guid> CreateBet(Bet input);
    }
}
