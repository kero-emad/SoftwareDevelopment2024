using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace National_Train_Reservation.Models
{

    
    public class Tickets
    {
        [Key]
        public int ID { get; set; }
        public DateTime Date_pickup { get; set; }
        public TimeSpan Time_Pickup { get; set; }
        public string Pickup_Station { get; set; }
        public string Destination { get; set; }
        public int Seat_Number { get; set; }
        public double Ticket_Money { get; set; }
        public string Class { get; set; }
        public int Train_Car_Number { get; set; }
        public virtual Users Users { get; set; }
        public int User_ID { get; set; }
        public virtual Trains Trains { get; set; }
        public int Journey_ID { get; set; }




    }
}
