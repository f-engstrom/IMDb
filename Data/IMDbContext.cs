using System;
using System.Collections.Generic;
using System.Text;
using IMDb.Domain;
using Microsoft.EntityFrameworkCore;

namespace IMDb.Data
{
    class IMDbContext : DbContext
    {
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Actor> Actor { get; set; }

        public DbSet<Address> Address { get; set; }

        public  DbSet<MovieActor> MovieActor { get; set; }

        public  DbSet<ProductionCompany> ProductionCompany { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=IMDb;Integrated Security=True");

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new {ma.ActorId, ma.MovieId});

            modelBuilder.Entity<MovieActor>()
                .HasOne(a => a.Actor)
                .WithMany(m => m.Movies)
                .HasForeignKey(a => a.ActorId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(m => m.movie)
                .WithMany(a => a.Actors)
                .HasForeignKey(m => m.MovieId);

        }
    }
}
