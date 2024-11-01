using System.ComponentModel.DataAnnotations;

namespace National_Train_Reservation.Models
{
    public enum Gender
    {
        Male, Female
    }
    
    
    public class Users
    {
        [Key]
        public int User_ID { get; set; }
        public long National_Pass_Number { get; set; }
        public string User_Fname { get; set; }
        public string User_Lname { get; set; }
        
      //  public string saltpassword {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string User_phone_number { get; set; }

        
        public Gender Gender{ get; set; }
        public string imagepath {  get; set; }

        public string type {  get; set; }

        public ICollection<Tickets> Tickets { get; set; }
        public ICollection<Payment> Payment { get; set; }

        public ICollection<Suggestions_Complaints> Suggestions_Complaints { get; set; }





    }
}
