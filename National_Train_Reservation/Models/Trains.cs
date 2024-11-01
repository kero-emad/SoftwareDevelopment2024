using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace National_Train_Reservation.Models
{
   
    public class Trains
    {
        [Key]
        public int Journey_ID { get; set; }
        public int TrainNumber { get; set; }
        public int Number_of_available_tickets { get; set; }
        public string Class { get; set; }
        public string Launching_Station { get; set; }
        public string Destination { get; set; }
        
        public DateTime Date_Pickup { get; set; }
        public TimeSpan Time_Pickup { get; set; }
        public int Number_of_stoppages { get; set; }
        public int Journey_Price { get; set; }
        public TimeSpan Arrival_Time { get; set; }

        
        public ICollection<AvailableTickets>AvailableTickets { get; set; }

        public ICollection<Tickets> Tickets { get; set; }

    }
}
