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
using Microsoft.AspNetCore.Hosting;
using AnimAlerte.ViewModels;
using AnimAlerte.Controllers;
using System.IO;
using Microsoft.AspNetCore.Mvc.Localization;

namespace AnimAlerte.Controllers
{
//tout est beau
    public class DetailsContactsController : Controller
    {
        private readonly IHtmlLocalizer<DetailsContactsController> _localizer;
        private readonly AnimAlerteContext _context;
        private readonly ISession session;
        public static string usersession;
        public static int admin = 0;

        public DetailsContactsController(AnimAlerteContext context, IHttpContextAccessor accessor, IHtmlLocalizer<DetailsContactsController> localizer)
        {
            _context = context;
            this.session = accessor.HttpContext.Session;
            _localizer = localizer;
        }

        // GET: DetailsContacts
        public IActionResult Index()
        {
            var listeContactsFavoris = _context.DetailsContacts.Where(a => a.NomUtilisateurCreateur == UtilisateursController.usersession && a.isFavoris==1).ToList();
            var listeUsers = _context.Utilisateurs.ToList();

            List<DetailsContact> detailsContactsFavoris = new List<DetailsContact>();
            if (listeContactsFavoris != null && listeUsers!=null)
            {
                foreach (var contact in listeContactsFavoris)
                {
                    foreach (var user in listeUsers)
                    {
                        if (contact.NomUtilisateurFavoris == user.NomUtilisateur && user.UtilisateurActive == 1 && user.NomUtilisateur != UtilisateursController.usersession)
                        {
                            detailsContactsFavoris.Add(contact);
                        }
                    }
                }
                return View(detailsContactsFavoris);
            }

            return View();
        }

        // GET: DetailsContacts Resultats Recherche
        public IActionResult Search(string utilisateursSearch)
        {
            ViewBag.Message = "";
            TempData["AlertMessage"] = "";
            List<Utilisateur> liste = new List<Utilisateur>();
            var listeToutUtilisateurs = _context.Utilisateurs
                .Where(u => u.NomUtilisateur != UtilisateursController.usersession && 
                u.UtilisateurActive == 1 && u.IsAdmin!=1).ToList();
            
            
            
            var listeTrouveUtilisateur = _context.Utilisateurs
                .Where(a => a.NomUtilisateur.Contains(utilisateursSearch) || 
                a.Nom.Contains(utilisateursSearch) || 
                a.Prenom.Contains(utilisateursSearch)).ToList();
                        
            if (utilisateursSearch == null)
            {
                return View(listeToutUtilisateurs);
            }
            else
            {
                foreach (var utilisateur in listeTrouveUtilisateur)
                {

                    if (utilisateur.UtilisateurActive == 1 && utilisateur.NomUtilisateur != UtilisateursController.usersession)
                    {
                        liste.Add(utilisateur);
                        TempData["AlertMessage"] = _localizer["SearchResults"];
                        //ViewBag.Message = _localizer["SearchResults"];
                    }
                } 
                if (liste.Count == 0)
                {
                    ViewBag.Message = _localizer["NoResults"];
                }
                return View(liste);
            }                    
        }

        // GET: DetailsContacts/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var detailsContact = _context.DetailsContacts.SingleOrDefault(c => c.NomUtilisateurFavoris == id);
            return View(detailsContact);
        }

        // GET: DetailsContacts/Create
        [HttpGet]
        public IActionResult Create()
        {
           return View();
        }
                   
        // POST: DetailsContacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string NomUtilisateurCreateur, string NomUtilisateurFavoris)
        {
            if (ModelState.IsValid)
            {
                DetailsContact obj = new DetailsContact();
                obj.NomUtilisateurCreateur = NomUtilisateurCreateur;
                obj.NomUtilisateurFavoris = NomUtilisateurFavoris;
                //obj.DateAjout = DateAjout;

                _context.DetailsContacts.Add(obj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NomUtilisateurCreateur"] = NomUtilisateurCreateur;
            ViewData["NomUtilisateurFavoris"] = NomUtilisateurFavoris; 
            return View();
        }

        // GET: DetailsContacts/Edit/5
        public async Task<IActionResult> Edit(string id1, string id2)
        {
            if (id1 == null)
            {
                return NotFound();
            }

            var detailsContact1 = await _context.DetailsContacts.FindAsync(id1);
            if (detailsContact1 == null)
            {
                return NotFound();
            }if (id2 == null)
            {
                return NotFound();
            }

            var detailsContact2 = await _context.DetailsContacts.FindAsync(id1);
            if (detailsContact1 == null)
            {
                return NotFound();
            }
            //ViewData["NomUtilisateurCreateur"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", detailsContact.NomUtilisateurCreateur);
            //ViewData["NomUtilisateurFavoris"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", detailsContact.NomUtilisateurFavoris);
            return View(detailsContact1);
        }

        // POST: DetailsContacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NomUtilisateurCreateur,NomUtilisateurFavoris,DateAjout")] DetailsContact detailsContact)
        {
            if (id != detailsContact.NomUtilisateurCreateur)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailsContact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailsContactExists(detailsContact.NomUtilisateurCreateur))
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
            ViewData["NomUtilisateurCreateur"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", detailsContact.NomUtilisateurCreateur);
            ViewData["NomUtilisateurFavoris"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", detailsContact.NomUtilisateurFavoris);
            return View(detailsContact);
        }

        // GET: DetailsContacts/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailsContact = _context.DetailsContacts.FirstOrDefault(m => m.NomUtilisateurFavoris == id);
            if (detailsContact == null)
            {
                return NotFound();
            }

            return View(detailsContact);
        }

        // POST: DetailsContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var detailsContact = _context.DetailsContacts.Find(id);
            _context.DetailsContacts.Remove(detailsContact);
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool DetailsContactExists(string id)
        {
            return _context.DetailsContacts.Any(e => e.NomUtilisateurCreateur == id);
        }
               
        // POST: DetailsContacts / Rajouter Contact Favoris
        public IActionResult Rajouter(string NomUtilisateur, string NomUtilisateurCreateur)
        {
            if (ModelState.IsValid)
            {
                DetailsContact obj = new DetailsContact();
                obj.NomUtilisateurFavoris = NomUtilisateur;

                obj.NomUtilisateurCreateur = UtilisateursController.usersession;
                
                //obj.NomUtilisateurCreateur = "user5";// hard coded
                //obj.NomUtilisateurCreateur = NomUtilisateurCreateur;
                obj.isFavoris = 1;
                obj.DateAjout = DateTime.Today;
                
                _context.Add(obj);
                _context.SaveChanges();
                
                TempData["AlertMessageContact"] = "Contact added";
                
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        // DELETE
        public IActionResult Supprimer(string NomUtilisateur, string NomUtilisateurCreateur)
        {
            if (ModelState.IsValid)
            {
                DetailsContact obj = new DetailsContact();
                obj.NomUtilisateurFavoris = NomUtilisateur;
                obj.NomUtilisateurCreateur = NomUtilisateurCreateur;
                obj.DateAjout = DateTime.Today;
                obj.isFavoris = 0;
                
                _context.Update(obj);
                _context.SaveChanges();
                //TempData["AlertMessageContact"] = _localizer["RemovedContact"];
                TempData["AlertMessageContact"] = "Contact Removed";               
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

    }
}


