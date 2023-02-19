using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.LibraryViewModels
{
    public class ReservationGroup
    {
        [DataType(DataType.Date)]
        public DateTime? ReservedDate { get; set; }
        public int MovieCount { get; set; }
    }
}
