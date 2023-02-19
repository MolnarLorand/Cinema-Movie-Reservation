/*using Cinema.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CinemaContext(serviceProvider.GetRequiredService<DbContextOptions<CinemaContext>>()))
            {
                if (context.Movie.Any())
                {
                    return;
                }
                context.Movie.AddRange(
                new Movie
                {
                    Title = "Hamlet",
                    Director="",
                    Genre = "Tragedy",
                    Price = Decimal.Parse("85")
                },

                new Movie
                {
                    Title = "Death of Salesman",
                    Director = context.Movie.First().Director,
                    Genre = "Drama",
                    Price = Decimal.Parse("56")
                },

                new Movie
                {
                    Title = "The Effect",
                    Director = context.Movie.First().Director,
                    Genre = "Romance",
                    Price = Decimal.Parse("48")
                }

                );


                context.Customers.AddRange(
                new Customer
                {
                    FirstName = "Test",
                    LastName = "Test",
                    Email = "test@yahoo.com",
                },
                new Customer
                {
                    FirstName = "Ana",
                    LastName = "pop",
                    Email = "ana@yahoo.com"
                }
                );
                context.SaveChanges();
            }
        }
    }
}
*/