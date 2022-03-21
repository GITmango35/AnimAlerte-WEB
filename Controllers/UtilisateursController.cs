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

namespace AnimAlerte.Controllers
{
    public class UtilisateursController : Controller
    {
        private readonly AnimAlerteContext _context;
        private readonly ISession session;
        public static string usersession;
        public static int admin = 0;

        public UtilisateursController(AnimAlerteContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            this.session = accessor.HttpContext.Session;
        }

        // GET: Utilisateur
        public IActionResult Index()
        {

            //session.SetString("NomUtilisateur", nomuser);
            //var user = UtilisateursController.usersession;
            //var user = "eli";// HARD-CODED -> TO BE MODIFIED
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

                    ViewBag.Message = "Vous etes bien enregistré";
                    return RedirectToAction("Login", "Utilisateurs", new { msg = ViewBag.Message });
                }
                else
                {
                    ViewBag.Message = "Le nom d'utilisateur existe deja!";
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
            // Verify this 
            // ViewData["NomAdmin"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", Utilisateurs.NomUtilisateur);
            return View(Utilisateurs);
        }
        // POST: Utilisateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NomUtilisateur,Nom,Prenom,Courriel,MotDePasse,NumTel,UtilisateurActive,IsAdmin,NomAdminDesactivateur")] Utilisateur utilisateur)
        {
            if (id != utilisateur.NomUtilisateur)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilisateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilisateurExists(utilisateur.NomUtilisateur))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
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
            //_context.Utilisateurs.Remove(utilisateur);
            await _context.SaveChangesAsync();
            session.Clear();
            usersession = "";
            return RedirectToAction("Login");
            //return RedirectToAction(nameof(Index));
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

        //---Methode Login
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Login(string nomuser, string mdp)
        {
            ViewBag.Message = "";

            Utilisateur utilisateur = _context.Utilisateurs.Find(nomuser);
            try
            {
                if (utilisateur != null && utilisateur.MotDePasse == mdp)

                {
                    session.SetString("NomUtilisateur", nomuser);
                    usersession = nomuser;
                    if (utilisateur.IsAdmin == 0)
                    {
                        admin = 0;
                        
                        return RedirectToAction("Index", "Annonces", new { nomuser = nomuser });
                    }
                    else
                    {
                        admin = 1;
                        return RedirectToAction("AllAnnoncesAdmin", "Annonces", new { nomuser = nomuser });
                       
                    }
                }

                else
                {
                    ViewBag.Message = "Nom Utilisateur ou mot de passe est incorrect!!";
                    return RedirectToAction("Login", new { msg = ViewBag.Message });
                }
            }
            catch (Exception)
            {
                
                return RedirectToAction("Index", "Utilisateurs");
            }



        }
        //Deconnexion
        [HttpPost]
        public IActionResult Logout()
        {
            session.Clear();
            usersession = "";
            return RedirectToAction("Login","Utilisateurs");
        }
    }
}
