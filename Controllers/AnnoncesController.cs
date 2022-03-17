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
                return RedirectToAction(nameof(TousMesAnnonces));
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
        //l'administrateur peut rechercher une annonce afin de la desactiver
        public ActionResult RechercheAnnonce()
        {
            return View();
        }



        [HttpPost]
        public ActionResult RechercheAnnonce(int idAnnonce)
        {
            var annonce = _context.Annonces.Find(idAnnonce);
            ViewBag.animals = _context.Animals.ToList();
            return View(annonce); //recuperer les infos d'annonce
        }



        //la désactivation d'annonce par un admin
        public ActionResult DesactiverAnnonce(int idAnnonce)
        {
            var annonce = _context.Annonces.SingleOrDefault(a => a.IdAnnonce == idAnnonce);
            ViewBag.admin = UtilisateursController.usersession;
            return View(annonce);
        }



        [HttpPost]
        public ActionResult DesactiverAnnonce(int idAnnonce, Annonce annonce)
        {
            var annonce1 = _context.Annonces.SingleOrDefault(a => a.IdAnnonce == idAnnonce);
            if (annonce1 != null)
            {



                annonce1.AnnonceActive = 0;
                _context.Entry(annonce1).State = EntityState.Modified;
                _context.SaveChanges();
            }



            return RedirectToAction("Index", "Annonces");
        }

        //Affichage de toute les annonces pour admin
        public IActionResult AllAnnoncesAdmin(string nomuser)
        {
            ViewBag.userSession = nomuser;
            var annonces = _context.Annonces.ToList();
            ViewBag.animaux = _context.Animals.ToList();
            ViewBag.images = _context.Images.ToList();
            return View(annonces);
        }
        // afficher toutes les annonces
        public IActionResult AllAnnonces(string nomuser)
        {
            ViewBag.userSession = nomuser;
            var annonces = _context.Annonces.ToList();
            ViewBag.animaux = _context.Animals.ToList();
            ViewBag.images = _context.Images.ToList();
            return View(annonces);
        }








    }
}
