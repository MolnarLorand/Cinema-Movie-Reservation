using Cinema.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Cinema.Data
{
    public class CinemaContext : DbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options) :
       base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Cynema> Cynemas { get; set; }
        public DbSet<CynemaMovie> CynemaMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Reservation>().ToTable("Reservation");
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<Director>().ToTable("Director");
            modelBuilder.Entity<Cynema>().ToTable("Cynema");
            modelBuilder.Entity<CynemaMovie>().ToTable("CynemaMovie");

            modelBuilder.Entity<CynemaMovie>().HasKey(c => new { c.MovieID, c.CynemaID });//configureaza cheia primara compusa

        }
    }
}
