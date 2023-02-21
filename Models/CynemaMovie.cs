using System.Security.Policy;

namespace Cinema.Models
{
    public class CynemaMovie
    {
        public int CynemaID { get; set; }
        public int MovieID { get; set; }
        public Cynema Cynema { get; set; }
        public Movie Movie { get; set; }

    }
}
