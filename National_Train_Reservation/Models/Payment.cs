using System.ComponentModel.DataAnnotations;

namespace National_Train_Reservation.Models
{
   
    public class Payment
    {
        [Key]
        public int ID { get; set; }
        public string  Card_Number { get; set; }
        public DateTime Expiration_date { get; set; }

        public int CVV { get; set; }
        public string Card_Owner_name { get; set; }
        public string User_Phone_number { get; set; }
        public virtual Users Users { get; set; }
        public int User_ID {  get; set; }

      
       

    }
}
