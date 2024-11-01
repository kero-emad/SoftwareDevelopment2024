using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using National_Train_Reservation.Data;
using National_Train_Reservation.Migrations;
using National_Train_Reservation.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace National_Train_Reservation.Controllers
{

    public class TicketsController : Controller
    {
        private readonly ApplicationDBcontext _con;
        private Trains _train;
        public TicketsController(ApplicationDBcontext con)
        {
            _con = con;
        }


        [HttpGet]
        public IActionResult Search_Ticket()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Search_Ticket(IFormCollection req)
        {
            string From = req["From"];
            string To = req["To"];
            String Date = req["Date"];
            string Class = req["Class"];
            DateTime date = DateTime.Parse(Date);
            var Trains = _con.Trains.Where(T => T.Launching_Station == From && T.Destination == To && T.Date_Pickup == date && T.Number_of_available_tickets > 0).ToList();
            if (date < DateTime.Today)
            {
                ModelState.AddModelError("", "Data has passed please Enter date later");
                return View();
            }
            if (Trains.Any())
            {
                return View("Search_Result", Trains);
            }

            else
            {
                ModelState.AddModelError("", "No Journies in this time or no available tickets");
                return View();
            }

        }

       
        [HttpGet]
        public IActionResult Book()
        {
            try
            {
                //  string journey = Request.Query["id"];
                //   int journey_id = int.Parse(journey);
                int journey_id = int.Parse(Request.Query["id"]);
                // int car_number = int.Parse( req["Car_number"]);
                //int seat_number = int.Parse( req["Seat_number"]);
                //TempData["car number"] = car_number;
                //TempData["seat number"]=seat_number;

                var ticket = _con.AvailableTickets.Where(At => At.Journey_ID == journey_id && At.IsBooked == false).FirstOrDefault();
                int car_number = ticket.carNumber;
                int seat_number = ticket.seatNumber;

                //ViewBag.CarNumber = car_number;
                //ViewBag.SeatNumber = seat_number;
                TempData["car number"] = car_number;
                TempData["seat number"] = seat_number;
                ViewBag.car = car_number;
                ViewBag.seat = seat_number;
                return View();
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("", "No Ticket choosen please search first");
                return RedirectToAction ("Search_Ticket");
            }
        }



        [HttpPost]
        public IActionResult Book(IFormCollection req)
        {
            string journey = req["Journey_ID"];
            int journey_id = int.Parse(journey);
            var train = _con.Trains.Where(t => t.Journey_ID == journey_id).FirstOrDefault();
            string serializedRes = JsonConvert.SerializeObject(train);
            HttpContext.Session.SetString("TrainRes", serializedRes);
            //ViewBag.Trainnumber;
            return RedirectToAction("Payment");

        }


        [HttpGet]
        public IActionResult Payment()
        {
            // var result = train.FirstOrDefault();
            //ViewBag.id = result.Journey_ID;
            return View();
        }


        [HttpPost]
        public IActionResult Payment(IFormCollection req)
        {


            string cardnumber = req["cardNumber"];
            int CVV = int.Parse(req["CVV"]);
            string date = req["Date"];
            DateTime expdate = DateTime.Parse(date);
            string name = req["name"];
            string phone = req["phone"];
            string Email = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = _con.Users.Where(u => u.Email == Email).FirstOrDefault();
            bool IsValidCardNumber(string cardnumber)
            {
                string regular = @"^\d{16}$";
                return Regex.IsMatch(cardnumber, regular);
            }

            bool IsValidPhone(string phone)
            {
                string regular = @"^01\d{9}$";
                return Regex.IsMatch(phone, regular);
            }
            bool IsValidName(string name)
            {
                string regular = @"^[A-Za-z\s]{3,}$";
                return Regex.IsMatch(name, regular);    
            }

            if (string.IsNullOrEmpty(phone) || !IsValidPhone(phone))
            {
                ModelState.AddModelError("", "The phone must be 11 numbers starts with 01");
                return View();
            }
            if (string.IsNullOrEmpty(cardnumber) || !IsValidCardNumber(cardnumber))
            {
                ModelState.AddModelError("", "The card number must be 16 numbers");
                return View();
            }
            if (string.IsNullOrEmpty(name) || !IsValidName(name))
            {
                ModelState.AddModelError("", "The name must be letters only at least 3 letters");
                return View();
            }

            if (expdate < DateTime.Today)
            {
                ModelState.AddModelError("", "The card is expired please try another card to finish the payment process");
                return View();
            }
            
            try {
                string serializedRes = HttpContext.Session.GetString("TrainRes");
               var Train = JsonConvert.DeserializeObject<Trains>(serializedRes);
                int car_number = (int)TempData["car number"];
                int seat_number = (int)TempData["seat number"];
                Payment payment = new Payment()
                {
                    Card_Number = cardnumber,
                    CVV = CVV,
                    Expiration_date = expdate,
                    Card_Owner_name = name,
                    User_Phone_number = phone,
                    User_ID = res.User_ID
                };
                _con.Payment.Add(payment);
                _con.SaveChanges();

                Trains updatedTrain = _con.Trains.FirstOrDefault(t => t.Journey_ID == Train.Journey_ID);
                if (updatedTrain != null)
                {
                    updatedTrain.Number_of_available_tickets -= 1;
                }
                _con.Trains.Update(updatedTrain);
                _con.SaveChanges();

                AvailableTickets updatedTicket = _con.AvailableTickets.FirstOrDefault(At => At.carNumber == car_number && At.seatNumber == seat_number && At.Journey_ID == Train.Journey_ID);
                if (updatedTicket != null)
                {
                    updatedTicket.IsBooked = true;
                }
                _con.AvailableTickets.Update(updatedTicket);
                _con.SaveChanges();

                Tickets Tickets = new Tickets()
                {
                    Journey_ID = Train.Journey_ID,
                    Time_Pickup = Train.Time_Pickup,
                    Date_pickup = Train.Date_Pickup,
                    Pickup_Station = Train.Launching_Station,
                    Destination = Train.Destination,
                    Ticket_Money = Train.Journey_Price,
                    Class = Train.Class,
                    Train_Car_Number = car_number,
                    Seat_Number = seat_number,
                    User_ID = res.User_ID,

                };
                _con.Tickets.Add(Tickets);
                _con.SaveChanges();
                return View("Ticket");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "No Ticket choosen please search ticket first then choose ticket if exists");
                return View();
            }
            
        }
        




        [HttpGet]

        public IActionResult View_Tickets()
        {
            if (User.Identity.IsAuthenticated)
            {
                string Email = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _con.Users.Where(u => u.Email == Email).FirstOrDefault();
                var ticket = _con.Tickets.Where(t => t.User_ID == user.User_ID).ToList();
                return View(ticket);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult View_Tickets(IFormCollection req)
        {
            int ticketid = (int)TempData["tick_id"];
            int car = (int)TempData["tick_car"];
            int seat= (int)TempData["tick_seat"];
            int jourid = (int)TempData["jour_id"];
            var deletedticket = _con.Tickets.FirstOrDefault(t => t.ID == ticketid);
            _con.Tickets.Remove(deletedticket);
            _con.SaveChanges();

            var updticket = _con.AvailableTickets.FirstOrDefault(At => At.carNumber == car && At.seatNumber == seat && At.Journey_ID == jourid);
            if (updticket != null)
            {
                updticket.IsBooked = false;
            }
            _con.AvailableTickets.Update(updticket);
            _con.SaveChanges();


            var updtrain=_con.Trains.FirstOrDefault(tr=>tr.Journey_ID==jourid);
            if (updtrain != null)
            {
                updtrain.Number_of_available_tickets += 1;
            }
            _con.Trains.Update(updtrain);
            _con.SaveChanges();



            return RedirectToAction("index","Home");
        }



        /*
         [HttpPost]
        public IActionResult Cancellation(IFormCollection req)
        {
            int ticketID = int.Parse(req["ticketId"]);
            var ticket = _con.Tickets.FirstOrDefault(t => t.ID == ticketID);
            _con.Tickets.Remove(ticket);
            _con.SaveChanges();
            return RedirectToAction("index", "Home");
        }
        */
    }
}
