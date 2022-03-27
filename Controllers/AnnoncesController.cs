using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimAlerte.Models;
using Microsoft.AspNetCore.Http;
//test2
namespace AnimAlerte.Controllers
{
    public class AnnoncesController : Controller
    {
        private readonly AnimAlerteContext _context;
        private readonly ISession session;

        public AnnoncesController(AnimAlerteContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            this.session = accessor.HttpContext.Session;
        }

        // GET: Annonces
        public async Task<IActionResult> Index()
        {
            var animAlerteContext = _context.Annonces.Include(a => a.IdAnimalNavigation).Include(a => a.NomAdminDesactivateurNavigation).Include(a => a.NomUtilisateurNavigation);
            return View(await animAlerteContext.ToListAsync());
        }

        public IActionResult TousMesAnnonces()
        {
            var nomuser = session.GetString("NomUtilisateur");
            var mesAnnonces = _context.Annonces.Where(a => a.NomUtilisateur == nomuser && a.AnnonceActive == 1).ToList();
            return View(mesAnnonces);
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
            ViewBag.userSession = session.GetString("NomUtilisateur");
            ViewBag.animaux_User = _context.Animals.Where(a => a.Proprietaire == session.GetString("NomUtilisateur")).ToList();
            return View();
        }

        // POST: Annonces/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Annonce annonce)
        {
            try
            {
                annonce.NomUtilisateur = session.GetString("NomUtilisateur");
                _context.Add(annonce);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Annonces/Edit/5
        public async Task<IActionResult> ModifierMonAnnonce(int? idAnnonce)
        {

            if (idAnnonce == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonces.FindAsync(idAnnonce);
            ViewBag.animaux_User = _context.Animals.Where(a => a.Proprietaire == session.GetString("NomUtilisateur")).ToList();
            if (annonce == null)
            {
                return NotFound();
            }
            return View(annonce);
        }

        // POST: Annonces/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModifierMonAnnonce(Annonce annonce)
        {
            if (ModelState.IsValid)
            {
                var annonceModif = _context.Annonces.Attach(annonce);
                annonceModif.State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction(nameof(TousMesAnnonces));
            }
            return View(annonce);
        }

        // GET: Annonces/Delete/5
        public async Task<IActionResult> DesactiverMonAnnonce(int? idAnnonce)
        {
            if (idAnnonce == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonces.FindAsync(idAnnonce);
            ViewBag.animal = await _context.Animals.FindAsync(annonce.IdAnimal);
            if (annonce == null)
            {
                return NotFound();
            }

            return View(annonce);
        }

        // POST: Annonces/Delete/5
        [HttpPost, ActionName("DesactiverMonAnnonce")]
        [ValidateAntiForgeryToken]
        public IActionResult DesactivationConfirmed(int idAnnonce)
        {
            var annonce = _context.Annonces.Find(idAnnonce);
            annonce.AnnonceActive = 0;
            var annonceModif = _context.Annonces.Attach(annonce);
            annonceModif.State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction(nameof(TousMesAnnonces));
        }
    }
}
