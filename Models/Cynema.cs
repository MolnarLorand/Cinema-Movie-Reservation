using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Cynema
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Cinema Name")]
        [StringLength(50)]
        public string CinemaName { get; set; }

        [StringLength(70)]
        public string Adress { get; set; }
        public ICollection<CynemaMovie> CynemaMovies { get; set; }
    }
}
