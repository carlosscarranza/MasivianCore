using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Core.Enums;
using Utilities.Core.Implementation.Models;

namespace Domain.Core.Classes
{
    [Table("Roulette")]
    public class Roulette : FullAuditedEntity<Guid>
    {
        public bool State { get; set; }

        [Required]
        public RouletteType Type { get; set; }
        public string Result { get; set; }
        public string DateOpening { get; set; }
        public string DateClosing { get; set; }
        public ICollection<Bet> Bets { get; set; }
    }
}
