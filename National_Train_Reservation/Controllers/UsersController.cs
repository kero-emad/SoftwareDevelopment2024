using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using National_Train_Reservation.Data;
using National_Train_Reservation.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;
using System.Security.Claims;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Drawing;
using NuGet.Packaging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
namespace National_Train_Reservation.Controllers
{
  
    public class UsersController : Controller
    {
       

        int currentcustomer;
        private readonly ApplicationDBcontext _con;
        private readonly IWebHostEnvironment _environment;
       
        public UsersController(ApplicationDBcontext con,IWebHostEnvironment environment)
        {
            _con = con;
            _environment = environment;

        }

       

        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            ViewBag.message = "";
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> SignUp(IFormCollection req,IFormFile img)
        {
             

            string email = req["email"];
            string pass = req["password"];
            string fname = req["Frist Name"];
            string lname = req["Last name"];
            string phone = req["Phone Number"];
            string national = req["National Number"];
            long nationalnum;
            long.TryParse(national, out nationalnum);
            string g = req["gender"];
            Gender gender =(Gender)Enum.Parse(typeof(Gender), g, true);
            string path = Path.Combine(_environment.WebRootPath, "Imgs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (img != null)
            {
                var allowed = new[] { ".jpg", ".jpeg" };
                var extension = Path.GetExtension(img.FileName);
                if (!allowed.Contains(extension.ToLower()))
                {
                    ModelState.AddModelError("", "Sorry you can upload image jpg or jpeg only");
                    return View();
                }
                path = Path.Combine(path, img.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                } 
            }
            else
            {
                ModelState.AddModelError("", "Nothing to upload");
                return View();
            }


           
            var user_email=_con.Users.FirstOrDefault(x => x.Email == email);
             bool IsValidEmail(string email)
            {

                string regular = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-z]{2,}$";
               return Regex.IsMatch(email, regular);
            }
            bool IsValidpassword(string password)
            {
                string regular = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{9,}";
                return Regex.IsMatch(password, regular);
            }
            bool IsValidname(string name)
            {
                string regular = "^[A-Za-z]{3,}$";
                return Regex.IsMatch(name, regular);
            }
            bool IsValidPhone(string phone)
            {
                string regular = @"^01\d{9}$";
                return Regex.IsMatch(phone, regular);
            }
            bool IsValidNational(string national)
            {
                string regular = @"^\d{14}$";
                return Regex.IsMatch(national, regular);
            }
            if (user_email != null)
            {
                ModelState.AddModelError("", "The email alrady exists please login or register with another account");
                return View();
            }
            if (fname.Length < 3 ||lname.Length<3)
            {
                ModelState.AddModelError("", "The name must be three letters or more");
                return View();

            }
            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
                {
                ModelState.AddModelError("", "The Email must be Valid ex(xxx@yahoo.com)");
                return View();
            }

            if (string.IsNullOrEmpty(pass) || !IsValidpassword(pass))
            {
                ModelState.AddModelError("", "The password must be at least 9 characters contating numbers,Capital,Small letters and sympols");
                return View();
            }
            if (string.IsNullOrEmpty(phone) || !IsValidPhone(phone))
            {

                ModelState.AddModelError("", "The Phone must be 11 numbers start with 01");
                return View();
            }
            if (string.IsNullOrEmpty(national) || !IsValidNational(national))
            {

                ModelState.AddModelError("", "The national number must be 14 numbers only");
                return View();
            }
            if (string.IsNullOrEmpty(fname)|| !IsValidname(fname))
            {
                ModelState.AddModelError("", "The first name must be letters at least 3 char");
                return View();
            }
            if (string.IsNullOrEmpty(lname) || !IsValidname(lname))
            {
                ModelState.AddModelError("", "The last name must be letters at least 3 char");
                return View();
            }

            string hashedPassword = HashPassword(pass);

            Users user = new Users() { User_Fname = fname,
                User_Lname = lname,
                User_phone_number = phone,
                National_Pass_Number = nationalnum,
                Email = email,
                Password = hashedPassword,
                Gender = gender,
                imagepath =img.FileName,
                type="user"

            };
            
            
                _con.Users.Add(user);
                _con.SaveChanges();
                return RedirectToAction("Signin", "Users");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        public ClaimsPrincipal GetUser()
        {
            return User;
        }

        [HttpPost]
        public async Task <IActionResult> SignIn(IFormCollection req)// ClaimsPrincipal User)
        {
            
            string email = req["email"];
            string pass = req["password"];

            string hashedpassword=HashPassword(pass);
            var user = _con.Users.FirstOrDefault(u => u.Email == email);
            

            if (user != null)
            {
                if (user.Password == hashedpassword)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Email)
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,

                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),properties);
                   return RedirectToAction("index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong password please try again");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Email is not registered please sign up");
                return View();
            }

        }
        
        public async Task <IActionResult>  SignOut()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "Home");
        }
        
        [HttpGet]
        public IActionResult Complaints_Suggestions()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Complaints_Suggestions(IFormCollection req)
        {
            string t = req["type"];
            MessageType type = (MessageType)Enum.Parse(typeof(MessageType), t, true);
            string subject = req["subject"];
            string details = req["message"];
            string Email=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res=_con.Users.Where(u=>u.Email==Email).FirstOrDefault();

            Suggestions_Complaints suggestions_Complaints = new Suggestions_Complaints()
            {
               UserID = res.User_ID,
                First_Name = res.User_Fname,
                Email = Email,
                Phone_Number = res.User_phone_number,
                Subject = subject,
                Details = details,
                Message_Type = type

            };
            _con.Suggestions_Complaints.Add(suggestions_Complaints);
            _con.SaveChanges();
            return RedirectToAction("Index", "Home");
            
        }


        [HttpGet]
        public IActionResult AddTrain()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTrain(IFormCollection req)
        {
            int trainnumber = int.Parse(req["trainNumber"]);
            string launchstation = req["launch"];
            string destination = req["destination"];
            string Class = req["Class"];
            int stop = int.Parse(req["stop"]);
            int price = int.Parse(req["price"]);
            DateTime date_pickup = DateTime.Parse(req["data_pickup"]);
            TimeSpan time_pickup = TimeSpan.Parse(req["time_pickup"]);
            TimeSpan arrival_time = TimeSpan.Parse(req["arrival_time"]);
            
            if (launchstation == destination)
            {
                ModelState.AddModelError("", "please choose two different stations");
                return View();
            }
            if (date_pickup <= DateTime.Today)
            {
                ModelState.AddModelError("", "The date is passed please try later date");
                return View();
            }
            Trains train = new Trains()
            {
                TrainNumber = trainnumber,
                Launching_Station = launchstation,
                Destination = destination,
                Class = Class,
                Number_of_stoppages = stop,
                Journey_Price = price,
                Number_of_available_tickets = 0,
                Date_Pickup = date_pickup,
                Time_Pickup = time_pickup,
                Arrival_Time = arrival_time,
            };
            _con.Trains.Add(train);
            _con.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult View_Trains()
        {
            var trains = _con.Trains.Where(tr=>tr.Date_Pickup>=DateTime.Today).ToList();
            return View(trains);
        }

        [HttpGet]
        public IActionResult AddTicket()
        {
           
            return View();
        }
        [HttpPost]
        public IActionResult AddTicket(IFormCollection req)
        {
            try {
                int jour = int.Parse(req["jourid"]);
                ViewBag.Jid = jour;
                int car = int.Parse(req["car"]);
                int seat = int.Parse(req["seat"]);
                var available = _con.AvailableTickets.Where(At => At.Journey_ID == jour).FirstOrDefault(At => At.carNumber == car && At.seatNumber == seat);

                if (available == null)
                {
                    AvailableTickets t = new AvailableTickets()
                    {
                        Journey_ID = jour,
                        carNumber = car,
                        seatNumber = seat,
                        IsBooked = false
                    };
                    _con.AvailableTickets.Add(t);
                    _con.SaveChanges();

                    var uptrain = _con.Trains.FirstOrDefault(t => t.Journey_ID == jour);
                    if (uptrain != null)
                    {
                        uptrain.Number_of_available_tickets += 1;
                    }
                    _con.Trains.Update(uptrain);
                    _con.SaveChanges();
                    ViewBag.Message = "Added Successfully";
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Car and seat already registered before");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Journey not exists please choose journey first then add ticket");
                return View();
            }
            }
        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Settings(IFormCollection req)
        {
            bool IsValidpassword(string password)
            {
                string regular = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{9,}";
                return Regex.IsMatch(password, regular);
            }
            string Email = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = _con.Users.Where(u => u.Email == Email).FirstOrDefault();
            string password = res.Password;
            string oldpassword = req["oldpass"];
            string hashedpassword = HashPassword(oldpassword);
            if (hashedpassword == password)
            {
                string newpassword = req["newpass"];
                if (string.IsNullOrEmpty(newpassword) || !IsValidpassword(newpassword))
                {
                    ModelState.AddModelError("", "The password must be at least 9 characters contating numbers,Capital,Small letters and sympols");
                    return View();
                }
                string hashednewpassword=HashPassword(newpassword);
                string confirmpass = req["confirmpass"];
                if (newpassword== confirmpass)
                {

                    res.Password = hashednewpassword;
                    _con.Users.Update(res);
                    _con.SaveChanges();
                    return RedirectToAction("index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Two passwords are not identical");
                    return View();
                }
            }
            else
            
                {
                    ModelState.AddModelError("", "The old password is wrong. Sorry you can't change the password ");
                    return View();
                }
           
        }


        [HttpGet]
        public IActionResult ViewTrains()
        {
            var trains = _con.Trains.Where(tr => tr.Date_Pickup >= DateTime.Today).ToList();
            return View(trains);
        }
        [HttpPost]
        public IActionResult ViewTrains(IFormCollection req)
        {
            int JID = (int)TempData["JID"];
            var deletetr=_con.Trains.FirstOrDefault(T=>T.Journey_ID == JID);
            _con.Trains.Remove(deletetr);
            _con.SaveChanges();
            return RedirectToAction("index", "Home");
        }

        
        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }
        

        [HttpPost]
        public IActionResult DeleteAccount(IFormCollection req)
        {
            string Email = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = _con.Users.Where(u => u.Email == Email).FirstOrDefault();
            _con.Users.Remove(res);
            _con.SaveChanges();
            HttpContext.Session.Clear();
            return RedirectToAction("index", "Home");
        }

        [HttpGet]
        public IActionResult updatePhoto()
        {
            if (User.Identity.IsAuthenticated)
            {
                string Email = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var res = _con.Users.Where(u => u.Email == Email).FirstOrDefault();
                res.imagepath = "deafult.jpeg";
                _con.Users.Update(res);
                _con.SaveChanges();
                return RedirectToAction("index", "Home");
            }
            else
            { return RedirectToAction("index", "home");
            }
        }
        
    }
}
