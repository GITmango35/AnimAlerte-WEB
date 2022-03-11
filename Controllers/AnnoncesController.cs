using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimAlerte.Models;

namespace AnimAlerte.Controllers
{
    public class AnnoncesController : Controller
    {
        private readonly AnimAlerteContext _context;

        public AnnoncesController(AnimAlerteContext context)
        {
            _context = context;
        }

        // GET: Annonces
        public async Task<IActionResult> Index()
        {
            var animAlerteContext = _context.Annonces.Include(a => a.IdAnimalNavigation).Include(a => a.NomAdminDesactivateurNavigation).Include(a => a.NomUtilisateurNavigation);
            return View(await animAlerteContext.ToListAsync());
        }

        // GET: Annonces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonces
                .Include(a => a.IdAnimalNavigation)
                .Include(a => a.NomAdminDesactivateurNavigation)
                .Include(a => a.NomUtilisateurNavigation)
                .FirstOrDefaultAsync(m => m.IdAnnonce == id);
            if (annonce == null)
            {
                return NotFound();
            }

            return View(annonce);
        }

        // GET: Annonces/Create
        public IActionResult Create()
        {
            ViewData["IdAnimal"] = new SelectList(_context.Animals, "IdAnimal", "DescriptionAnimal");
            ViewData["NomAdminDesactivateur"] = new SelectList(_context.Administrateurs, "NomAdmin", "NomAdmin");
            ViewData["NomUtilisateur"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur");
            return View();
        }

        // POST: Annonces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAnnonce,DateCreation,Titre,DescriptionAnnonce,Ville,AnnonceActive,TypeAnnonce,IdAnimal,NomUtilisateur,NomAdminDesactivateur")] Annonce annonce)
        {
            if (ModelState.IsValid)
            {
                _context.Add(annonce);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAnimal"] = new SelectList(_context.Animals, "IdAnimal", "DescriptionAnimal", annonce.IdAnimal);
            ViewData["NomAdminDesactivateur"] = new SelectList(_context.Administrateurs, "NomAdmin", "NomAdmin", annonce.NomAdminDesactivateur);
            ViewData["NomUtilisateur"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", annonce.NomUtilisateur);
            return View(annonce);
        }

        // GET: Annonces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonces.FindAsync(id);
            if (annonce == null)
            {
                return NotFound();
            }
            ViewData["IdAnimal"] = new SelectList(_context.Animals, "IdAnimal", "DescriptionAnimal", annonce.IdAnimal);
            ViewData["NomAdminDesactivateur"] = new SelectList(_context.Administrateurs, "NomAdmin", "NomAdmin", annonce.NomAdminDesactivateur);
            ViewData["NomUtilisateur"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", annonce.NomUtilisateur);
            return View(annonce);
        }

        // POST: Annonces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAnnonce,DateCreation,Titre,DescriptionAnnonce,Ville,AnnonceActive,TypeAnnonce,IdAnimal,NomUtilisateur,NomAdminDesactivateur")] Annonce annonce)
        {
            if (id != annonce.IdAnnonce)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(annonce);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnonceExists(annonce.IdAnnonce))
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
            ViewData["IdAnimal"] = new SelectList(_context.Animals, "IdAnimal", "DescriptionAnimal", annonce.IdAnimal);
            ViewData["NomAdminDesactivateur"] = new SelectList(_context.Administrateurs, "NomAdmin", "NomAdmin", annonce.NomAdminDesactivateur);
            ViewData["NomUtilisateur"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", annonce.NomUtilisateur);
            return View(annonce);
        }

        // GET: Annonces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonces
                .Include(a => a.IdAnimalNavigation)
                .Include(a => a.NomAdminDesactivateurNavigation)
                .Include(a => a.NomUtilisateurNavigation)
                .FirstOrDefaultAsync(m => m.IdAnnonce == id);
            if (annonce == null)
            {
                return NotFound();
            }

            return View(annonce);
        }

        // POST: Annonces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annonce = await _context.Annonces.FindAsync(id);
            _context.Annonces.Remove(annonce);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnonceExists(int id)
        {
            return _context.Annonces.Any(e => e.IdAnnonce == id);
        }
    }
}
