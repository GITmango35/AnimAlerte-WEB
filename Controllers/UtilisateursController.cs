using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimAlerte.Models;
using System.Data;

namespace AnimAlerte.Controllers
{
    public class UtilisateursController : Controller
    {
        private readonly AnimAlerteContext _context;

        public UtilisateursController(AnimAlerteContext context)
        {
            _context = context;
        }

        // GET: Utilisateurs
        public async Task<IActionResult> Index()
        {
            var animAlerteContext = _context.Utilisateurs.Include(u => u.NomAdminDesactivateurNavigation);
            return View(await animAlerteContext.ToListAsync());
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
                    return RedirectToAction("Index", "Home", new { msg = ViewBag.Message });
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

            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            ViewData["NomAdminDesactivateur"] = new SelectList(_context.Administrateurs, "NomAdmin", "NomAdmin", utilisateur.NomAdminDesactivateur);
            return View(utilisateur);
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
            _context.Utilisateurs.Remove(utilisateur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilisateurExists(string id)
        {
            return _context.Utilisateurs.Any(e => e.NomUtilisateur == id);
        }
    }
}
