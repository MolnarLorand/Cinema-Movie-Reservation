using System.Security.Policy;

namespace Cinema.Models.LibraryViewModels
{
    public class CynemaIndexData
    {
        public IEnumerable<Cynema> Cynemas { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }

    }
}
