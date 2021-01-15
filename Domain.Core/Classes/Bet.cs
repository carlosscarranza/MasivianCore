using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Core.Enums;
using Utilities.Core.Implementation.Models;

namespace Domain.Core.Classes
{
    [Table("Bet")]
    public class Bet : FullAuditedEntity<Guid>
    {
        [Required]
        public double Amount { get; set; }
        public int NumberBet { get; set; }
        public ColourType ColourBet { get; set; }

        [Required]
        public Guid RouletteId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("RouletteId")]
        public Roulette Roulette { get; set; }
        
    }
}