using System;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Core.Implementation.Models;

namespace Domain.Core
{
    [Table("Roulette")]
    public class Roulette : FullAuditedEntity<Guid>
    {
        public bool State { get; set; }
        public RouletteType Type { get; set; }
        public string Result { get; set; }
        public DateTime DateOpening { get; set; }
        public DateTime DateClosing { get; set; }
        public Guid BetId { get; set; }

        [ForeignKey("BetId")]
        public Bet Bet { get; set; }
    }
}
