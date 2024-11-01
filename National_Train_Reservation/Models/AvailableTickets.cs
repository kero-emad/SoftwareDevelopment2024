using System.ComponentModel.DataAnnotations;
namespace National_Train_Reservation.Models
{
    public class AvailableTickets
    {
        [Key]
        public int ID { get; set; }
        public int Journey_ID { get; set; }
        public int seatNumber { get; set; }
        public int carNumber { get; set; }
        public bool IsBooked { get; set; }
        public virtual Trains Trains { get; set; }
    }
}
