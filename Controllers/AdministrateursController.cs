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
    public class AdministrateursController : Controller
    {
        private readonly AnimAlerteContext _context;

        public AdministrateursController(AnimAlerteContext context)
        {
            _context = context;
        }

        // GET: Administrateurs
        public async Task<IActionResult> Index()
        {
            var animAlerteContext = _context.Administrateurs.Include(a => a.NomAdminNavigation);
            return View(await animAlerteContext.ToListAsync());
        }

        // GET: Administrateurs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrateur = await _context.Administrateurs
                .Include(a => a.NomAdminNavigation)
                .FirstOrDefaultAsync(m => m.NomAdmin == id);
            if (administrateur == null)
            {
                return NotFound();
            }

            return View(administrateur);
        }

        // GET: Administrateurs/Create
        public IActionResult Create()
        {
            ViewData["NomAdmin"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur");
            return View();
        }

        // POST: Administrateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomAdmin,DateCreation")] Administrateur administrateur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administrateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NomAdmin"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", administrateur.NomAdmin);
            return View(administrateur);
        }

        // GET: Administrateurs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrateur = await _context.Administrateurs.FindAsync(id);
            if (administrateur == null)
            {
                return NotFound();
            }
            ViewData["NomAdmin"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", administrateur.NomAdmin);
            return View(administrateur);
        }

        // POST: Administrateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NomAdmin,DateCreation")] Administrateur administrateur)
        {
            if (id != administrateur.NomAdmin)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administrateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministrateurExists(administrateur.NomAdmin))
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
            ViewData["NomAdmin"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", administrateur.NomAdmin);
            return View(administrateur);
        }

        // GET: Administrateurs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrateur = await _context.Administrateurs
                .Include(a => a.NomAdminNavigation)
                .FirstOrDefaultAsync(m => m.NomAdmin == id);
            if (administrateur == null)
            {
                return NotFound();
            }

            return View(administrateur);
        }

        // POST: Administrateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var administrateur = await _context.Administrateurs.FindAsync(id);
            _context.Administrateurs.Remove(administrateur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministrateurExists(string id)
        {
            return _context.Administrateurs.Any(e => e.NomAdmin == id);
        }
    }
}
