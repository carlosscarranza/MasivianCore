using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Core
{
    [Table("Bet")]
    public class Bet
    {
        public double Amount { get; set; }
        public int NumberBet { get; set; }
        public string ColourBet { get; set; }
        public Guid UserId { get; set; }
        public ICollection<Roulette> Roulettes { get; set; }
        public ICollection<User> Users { get; set; }
    }
}