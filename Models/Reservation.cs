namespace Cinema.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int CustomerID { get; set; }
        public int MovieID { get; set; }

        public DateTime ReservationDate { get; set; }
        public Customer? Customer { get; set; }
        public Movie? Movie { get; set; }
    }
}
