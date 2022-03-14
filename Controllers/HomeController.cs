using AnimAlerte.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AnimAlerte.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AnimAlerteContext _context;
        private readonly ISession session;
        public static string usersession;
        public static int admin = 0;

        public HomeController(ILogger<HomeController> logger, AnimAlerteContext context, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _context = context;
            this.session = accessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        

        public IActionResult Login(string msg)
        {

            ViewBag.Message = msg;
            return View();
        }




        //---Methode Authentication
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Login(string nomuser, string mdp)
        {
            ViewBag.Message = "";

            Utilisateur utilisateur = _context.Utilisateurs.Find(nomuser);
           try
            {
               // var u = _context.Utilisateurs.FirstOrDefault(u => u.NomUtilisateur == nomuser);
                if (utilisateur != null && utilisateur.MotDePasse == mdp)

                {

                    session.SetString("NomUtilisateur", nomuser);
                    usersession = nomuser;
                    if (utilisateur.IsAdmin == 0)
                    {
                        admin = 0;
                       // return RedirectToAction("Index", "Annonces", new { nomuser = nomuser });
                        return RedirectToAction("Index", "Utilisateurs");
                    }
                    else
                    {
                        admin = 1;
                        return RedirectToAction("Index", "Annonces", new { nomuser = nomuser });
                       // return RedirectToAction("Index", "Utilisaieurs");
                    }


                }

                else
                {
                    ViewBag.Message = "Nom Utilisateur ou mot de passe est incorrect!!";
                    return RedirectToAction("Index", new { msg = ViewBag.Message });
                }
            }
           catch (Exception)
            {
                return View();
            }



            /*  catch (DbUpdateConcurrencyException)
              {
                  return NotFound();
              }
            */

        }

        public IActionResult Profil()
        {
            return View();
        }


        //Deconnexion
        [HttpPost]
        public IActionResult Logout()
        {
            session.Clear();
            usersession = "";
            return RedirectToAction("Login");
        }



        












    }
}
