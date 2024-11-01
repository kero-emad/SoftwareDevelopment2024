using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;

namespace National_Train_Reservation.Models
{
    public enum MessageType
    {
        Suggestions, Complaints
    }
    public class Suggestions_Complaints
    {
        [Key]
        public int ID { get; set; }
        public MessageType? Message_Type { get; set; }
        public string First_Name { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public virtual Users Users { get; set; }
        public int UserID {  get; set; }

    }
}
