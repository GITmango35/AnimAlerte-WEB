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
    public class DetailsContactsController : Controller
    {
        private readonly AnimAlerteContext _context;

        public DetailsContactsController(AnimAlerteContext context)
        {
            _context = context;
        }

        // GET: DetailsContacts
        public async Task<IActionResult> Index()
        {
            var animAlerteContext = _context.DetailsContacts.Include(d => d.NomUtilisateurCreateurNavigation).Include(d => d.NomUtilisateurFavorisNavigation);
            return View(await animAlerteContext.ToListAsync());
        }

        // GET: DetailsContacts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailsContact = await _context.DetailsContacts
                .Include(d => d.NomUtilisateurCreateurNavigation)
                .Include(d => d.NomUtilisateurFavorisNavigation)
                .FirstOrDefaultAsync(m => m.NomUtilisateurCreateur == id);
            if (detailsContact == null)
            {
                return NotFound();
            }

            return View(detailsContact);
        }

        // GET: DetailsContacts/Create
        public IActionResult Create()
        {
            ViewData["NomUtilisateurCreateur"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur");
            ViewData["NomUtilisateurFavoris"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur");
            return View();
        }

        // POST: DetailsContacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomUtilisateurCreateur,NomUtilisateurFavoris,DateAjout")] DetailsContact detailsContact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailsContact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NomUtilisateurCreateur"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", detailsContact.NomUtilisateurCreateur);
            ViewData["NomUtilisateurFavoris"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", detailsContact.NomUtilisateurFavoris);
            return View(detailsContact);
        }

        // GET: DetailsContacts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailsContact = await _context.DetailsContacts.FindAsync(id);
            if (detailsContact == null)
            {
                return NotFound();
            }
            ViewData["NomUtilisateurCreateur"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", detailsContact.NomUtilisateurCreateur);
            ViewData["NomUtilisateurFavoris"] = new SelectList(_context.Utilisateurs, "NomUtilisateur", "NomUtilisateur", detailsContact.NomUtilisateurFavoris);
            return View(detailsContact);
        }

        // POST: DetailsContacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailsContact = await _context.DetailsContacts
                .Include(d => d.NomUtilisateurCreateurNavigation)
                .Include(d => d.NomUtilisateurFavorisNavigation)
                .FirstOrDefaultAsync(m => m.NomUtilisateurCreateur == id);
            if (detailsContact == null)
            {
                return NotFound();
            }

            return View(detailsContact);
        }

        // POST: DetailsContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var detailsContact = await _context.DetailsContacts.FindAsync(id);
            _context.DetailsContacts.Remove(detailsContact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetailsContactExists(string id)
        {
            return _context.DetailsContacts.Any(e => e.NomUtilisateurCreateur == id);
        }
    }
}
