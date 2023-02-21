using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public int? DirectorID { get; set; }
        public Director? Director { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<CynemaMovie> CynemaMovies { get; set; }

    }
}
