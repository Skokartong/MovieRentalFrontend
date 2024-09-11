using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieRentalFrontend.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int FK_UserId { get; set; }
        public int FK_MovieId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
