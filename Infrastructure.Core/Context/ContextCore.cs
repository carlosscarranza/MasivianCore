﻿using System.Linq;
using Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Utilities.Core.Implementation.Database;

namespace Infrastructure.Core.Context
{
    public class ContextCore : RepositoryDbContext
    {
        public ContextCore(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Roulette> Roulettes { get; set; }
        public virtual DbSet<Bet> Bets { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }
    }
}
