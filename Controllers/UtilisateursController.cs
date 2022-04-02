using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimAlerte.Models;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace AnimAlerte.Controllers
{
    public class UtilisateursController : Controller
    { 
        private readonly IStringLocalizer<UtilisateursController> _stringlocalizer;
        private readonly IHtmlLocalizer<UtilisateursController> _localizer;
        private readonly AnimAlerteContext _context;
        private readonly ISession session;
        public static string usersession;
        public static int admin = 0;

        public UtilisateursController(AnimAlerteContext context, IHttpContextAccessor accessor, IStringLocalizer<UtilisateursController> stringlocalizer, IHtmlLocalizer<UtilisateursController> localizer)
        {
            _context = context;
            _localizer = localizer;
            _stringlocalizer = stringlocalizer;
            this.session = accessor.HttpContext.Session;
        }

        // GET: Utilisateur
        public IActionResult Index()
        {
            var profil = _context.Utilisateurs.Where(u => u.NomUtilisateur == session.GetString("NomUtilisateur")).ToList();
            return View(profil);
        }

        // GET: Utilisateurs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _context.Utilisateurs
                .Include(u => u.NomAdminDesactivateurNavigation)
                .FirstOrDefaultAsync(m => m.NomUtilisateur == id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        public IActionResult Create()
        {
            ViewData["NomAdminDesactivateur"] = new SelectList(_context.Administrateurs, "NomAdmin", "NomAdmin");
            return View();
        }

        // POST: Utilisateurs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomUtilisateur,Nom,Prenom,Courriel,MotDePasse,NumTel,UtilisateurActive,IsAdmin,NomAdminDesactivateur")] Utilisateur utilisateur)
        {
            var u = _context.Utilisateurs.FirstOrDefault(u => u.NomUtilisateur == utilisateur.NomUtilisateur);

            ViewBag.Message = "";
            try
            {

                if (u == null && ModelState.IsValid)
                {
                    _context.Add(utilisateur);
                    await _context.SaveChangesAsync();
                    
                   
                    ViewBag.Message = "Bievenue sur le site AnimAlerte! Welcome to AnimAlerte!";
                    return RedirectToAction("Login", "Utilisateurs", new { msg = ViewBag.Message });
                }
                else
                {
                    
                    ViewBag.Message = "Le nom d'utilisateur existe deja!/ This User exist!";
                    return View(utilisateur);
                }

            }
            catch (DataException)
            {
                ModelState.AddModelError("", "On ne peut pas enregistrer");


            }
            ViewData["NomAdminDesactivateur"] = new SelectList(_context.Administrateurs, "NomAdmin", "NomAdmin", utilisateur.NomAdminDesactivateur);
            return View(utilisateur);
        }

        // GET: Utilisateurs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Utilisateurs = await _context.Utilisateurs.FindAsync(id);
            if (Utilisateurs == null)
            {
                return NotFound();
            }
            var message = "";
            ViewBag.Message = message;
            return View(Utilisateurs);
        }

        // POST: Utilisateurs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("NomUtilisateur,Nom,Prenom,Courriel,MotDePasse,NumTel,UtilisateurActive,IsAdmin,NomAdminDesactivateur")] Utilisateur utilisateur)
        {
            if (id != utilisateur.NomUtilisateur)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                TempData["AlertMessageProfil"] = "";
                utilisateur.UtilisateurActive = 1;
                utilisateur.IsAdmin = 0;
                _context.Update(utilisateur);
                _context.SaveChanges();
                TempData["AlertMessageProfil"] = "Modified";
                return RedirectToAction(nameof(Index));
            }
            ViewData["NomAdminDesactivateur"] = new SelectList(_context.Administrateurs, "NomAdmin", "NomAdmin", utilisateur.NomAdminDesactivateur);
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _context.Utilisateurs
                .Include(u => u.NomAdminDesactivateurNavigation)
                .FirstOrDefaultAsync(m => m.NomUtilisateur == id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            utilisateur.UtilisateurActive = 0;
            _context.Utilisateurs.Update(utilisateur);
            await _context.SaveChangesAsync();
            session.Clear();
            usersession = "";

            return RedirectToAction("Login");
        }

        private bool UtilisateurExists(string id)
        {
            return _context.Utilisateurs.Any(e => e.NomUtilisateur == id);
        }

        //Get Login
        public IActionResult Login(string msg)
        {
            ViewBag.Message = msg;
            return View();
        }

        //SE CONNECTER
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Login(string nomuser, string mdp)
        {
            ViewBag.Message = "";

            Utilisateur utilisateur = _context.Utilisateurs.Find(nomuser);
            try
            {
                if (utilisateur != null && utilisateur.MotDePasse == mdp && utilisateur.UtilisateurActive == 1)

                {
                    session.SetString("NomUtilisateur", nomuser);
                    usersession = nomuser;
                    if (utilisateur.IsAdmin == 0)
                    {
                        admin = 0;

                        return RedirectToAction("AllAnnoncesUser", "Annonces", new { nomuser = nomuser });
                    }
                    else
                    {
                        admin = 1;
                        return RedirectToAction("AllAnnoncesAdmin", "Annonces", new { nomuser = nomuser });

                    }
                }

                else
                {
                    ViewBag.Message = _localizer["LoginError"];
                    return RedirectToAction("Login", new { msg = ViewBag.Message });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Utilisateurs");
            }

        }
        //DECONEXION
        [HttpPost]
        public IActionResult Logout()
        {
            session.Clear();
            usersession = "";
            return RedirectToAction("Login", "Utilisateurs");
        }

        //------un administrateur peut rechercher un utilisateur afin de le désactiver 
        public ActionResult RechercheUtilisateur()
        {
            var listeUtilisateurs = _context.Utilisateurs.Where(u => u.UtilisateurActive == 1).ToList();
            if (listeUtilisateurs != null)
            {
                return View(listeUtilisateurs);
            }



            return View();
        }

        [HttpPost]
        public ActionResult RechercheUtilisateur(string nomuser)
        {
            var listeToutUtilisateurs = _context.Utilisateurs.Where(u => u.UtilisateurActive == 1).ToList();
            if (nomuser == null)
            {
                return View(listeToutUtilisateurs);                
            }
            else 
            {
                var utilisateur = _context.Utilisateurs.SingleOrDefault(u => u.NomUtilisateur == nomuser && u.UtilisateurActive == 1);
                if (utilisateur != null)
                {
                    ViewBag.userA = "";
                    if (utilisateur.IsAdmin == 1)
                    {
                        ViewBag.userA = _localizer["Admin"];
                    }
                    else
                    {
                        ViewBag.userA = _localizer["User"];
                    }
                    return View(utilisateur); //recuperer les infos d'User
                }
                return View();
            }
        }

        // La désactivation d'un utilisateur par un admin
        public ActionResult DesactiverUtilisateur(string nomuser)
        {
            var utilisateur = _context.Utilisateurs.SingleOrDefault(u=>u.NomUtilisateur == nomuser && u.UtilisateurActive == 1);
            ViewBag.admin = session.GetString("NomUtilisateur");
            return View(utilisateur);
        }

        [HttpPost]
        public ActionResult DesactiverUtilisateur( Utilisateur utilisateur)
        {
             var utilisateur1 = _context.Utilisateurs.SingleOrDefault(u => u.NomUtilisateur == utilisateur.NomUtilisateur && u.UtilisateurActive == 1);

            if (utilisateur1 != null)
            {
                //avant de désactiver l'utilisateur, on doit tt d'abord désactiver ses annonces et ses animaux
                    //-1---Désactivation de ses annonces
                    var annoncesUtilisateurs = _context.Annonces.Where(a => a.NomUtilisateur == utilisateur1.NomUtilisateur).ToList();
                    foreach (var annonce in annoncesUtilisateurs)
                    {
                        annonce.AnnonceActive = 0;
                        _context.Entry(annonce).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    //-2---Désactivation de ses animaux
                    var animauxUtilisateurs = _context.Animals.Where(a => a.Proprietaire == utilisateur1.NomUtilisateur).ToList();
                    foreach (var animal in animauxUtilisateurs)
                    {
                        animal.AnimalActif = 0;
                        _context.Entry(animal).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                //--désactivation de l'utilisateur
                utilisateur1.UtilisateurActive = 0;        
                _context.Entry(utilisateur1).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("AllAnnoncesAdmin", "Annonces");
        }
    }
}
