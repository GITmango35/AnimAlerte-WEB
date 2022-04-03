using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimAlerte.Models;
using Microsoft.AspNetCore.Http;
using AnimAlerte.ViewModels;
using Microsoft.Extensions.Localization;

namespace AnimAlerte.Controllers
{
    public class AnnoncesController : Controller
    {
        private readonly AnimAlerteContext _context;
        private readonly ISession session;
        private readonly IStringLocalizer<AnnoncesController> _stringLocalizer;

        public AnnoncesController(AnimAlerteContext context, IHttpContextAccessor accessor, IStringLocalizer<AnnoncesController> stringLocalizer)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
            this.session = accessor.HttpContext.Session;
        }

        // GET: Annonces
        // Index retour la liste des annonces, une barre de recherche par ville, filtrer par type d'annonce (perdu ou trouvé)
        public async Task<IActionResult> Index(string nomuser, string searchString, string annoncePerdu, string annonceTrouve)
        {

            ViewData["CurrentFilter"] = searchString;
            ViewData["LostAnimalFilter"] = annoncePerdu == "perdu" ? "trouve" : "perdu";
            ViewData["FoundAnimalFilter"] = annonceTrouve == "trouve" ? "perdu" : "trouve";


            ViewBag.userSession = nomuser;
            var annonces = from a in _context.Annonces
                           where a.AnnonceActive == 1
                           select a;
            ViewBag.animaux = _context.Animals.Where(a => a.AnimalActif == 1).ToList();
            ViewBag.images = _context.Images.ToList();



            if (!String.IsNullOrEmpty(searchString))
            {
                ViewData["SearchAdResult"] = _stringLocalizer["We found result(s)."].Value;
                annonces = annonces.Where(a => a.Ville.ToUpper().Contains(searchString.ToUpper()) && a.AnnonceActive == 1);
            }
            else
            {
                TempData["NoSearchAdResult"] = _stringLocalizer["We're sorry! We were not able to find a match."].Value;
            }

            if (!String.IsNullOrEmpty(annoncePerdu))
            {
                annonces = annonces.Where(a => a.TypeAnnonce == "perdu" && a.AnnonceActive == 1);
            }

            if (!String.IsNullOrEmpty(annonceTrouve))
            {
                annonces = annonces.Where(a => a.TypeAnnonce == "trouve" && a.AnnonceActive == 1);
            }

            return View(await annonces.ToListAsync());
        }


        public IActionResult TousMesAnnonces()
        {
            var nomUser = session.GetString("NomUtilisateur");
            var mesAnnonces = _context.Annonces.Where(a => a.NomUtilisateur == nomUser && a.AnnonceActive == 1).ToList();
            return View(mesAnnonces);
        }

        // GET: Annonces/Details/5
        public async Task<IActionResult> Details(int? idAnimal)
        {
            if (idAnimal == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonces
            .FirstOrDefaultAsync(m => m.IdAnnonce == idAnimal);

            var animal = _context.Animals.Include(m => m.ProprietaireNavigation)
                .FirstOrDefault(m => m.IdAnimal == idAnimal);

            var image = _context.Images
               .FirstOrDefault(m => m.IdImage == idAnimal);

            var user = _context.Utilisateurs.Find(annonce.NomUtilisateur);
            var name = user.Nom;
            var FirstName = user.Prenom;
            var phone = user.NumTel;
            var email = user.Courriel;
            ViewData["Name"] = name;
            ViewData["FirstName"] = FirstName;
            ViewData["Phone"] = phone;
            ViewData["Email"] = email;

            var model = new AnnonceModifViewModel()
            {
                IdAnnonce = annonce.IdAnnonce,
                Titre = annonce.Titre,
                DescriptionAnnonce = annonce.DescriptionAnnonce,
                Ville = annonce.Ville,
                DateCreation = annonce.DateCreation,
                AnnonceActive = 1,
                TypeAnnonce = annonce.TypeAnnonce,
                NomUtilisateur = annonce.NomUtilisateur,
                IdAnimal = animal.IdAnimal,
                NomAnimal = animal.NomAnimal,
                DescriptionAnimal = animal.DescriptionAnimal,
                DateInscription = animal.DateInscription,
                AnimalActif = 1,
                Proprietaire = animal.Proprietaire,
                PhotoPath = image.PathImage,
            };

            if (annonce == null)
            {
                return NotFound();
            }

            return View(model);
        }



        // GET: Annonces/Create
        public IActionResult Create()
        {
            ViewBag.userSession = session.GetString("NomUtilisateur");
            ViewBag.animaux_User = _context.Animals.Where(a => a.Proprietaire == session.GetString("NomUtilisateur") && a.AnimalActif == 1).ToList();
            return View();
        }

        // POST: Annonces/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Annonce annonce)
        {
            try
            {
                TempData["AlertMessage"] = "";
                annonce.NomUtilisateur = session.GetString("NomUtilisateur");
                _context.Add(annonce);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = _stringLocalizer["Your ad is added successfully !"].Value;
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
                TempData["AlertMessage"] = "";
                var annonceModif = _context.Annonces.Attach(annonce);
                annonceModif.State = EntityState.Modified;
                _context.SaveChanges();
                TempData["AlertMessage"] = _stringLocalizer["Your ad is modified successfully!"].Value;
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
            TempData["AlertMessage"] = "";
            var annonce = _context.Annonces.Find(idAnnonce);
            annonce.AnnonceActive = 0;
            var annonceModif = _context.Annonces.Attach(annonce);
            annonceModif.State = EntityState.Modified;
            _context.SaveChanges();
            TempData["AlertMessage"] = _stringLocalizer["Your ad is deleted successfully!"].Value;
            return RedirectToAction(nameof(TousMesAnnonces));
        }

        //L'administrateur peut rechercher une annonce afin de la desactiver
        public ActionResult RechercheAnnonce()
        {
            var listeAnnonces = _context.Annonces.Where(a => a.AnnonceActive == 1).ToList();
            ViewBag.animals = _context.Animals.ToList();
            return View(listeAnnonces);
        }

        [HttpPost]
        public ActionResult RechercheAnnonce(int idAnnonce)
        {
            var annonce = _context.Annonces.Where(a => a.IdAnnonce == idAnnonce && a.AnnonceActive == 1).ToList();
            ViewBag.animals = _context.Animals.ToList();

            if (annonce != null)
            {
                ViewData["AlertMessageAnnonces"] = _stringLocalizer["We found result(s)."].Value;
                return View(annonce); //recuperer les infos d'annonce
            }
            else
            {
                TempData["AlertMessageAnnonces"] = _stringLocalizer["Sorry, this ad does not exists !"].Value;
                return RedirectToAction(nameof(RechercheAnnonce));
            }

        }

        //la désactivation d'annonce par un admin
        public ActionResult DesactiverAnnonce(int idAnnonce)
        {
            var annonce = _context.Annonces.SingleOrDefault(a => a.IdAnnonce == idAnnonce && a.AnnonceActive == 1);
            ViewBag.admin = UtilisateursController.usersession;

            if (annonce != null)
            {
                return View(annonce);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult DesactiverAnnonce(int idAnnonce, Annonce annonce)
        {
            var annonce1 = _context.Annonces.SingleOrDefault(a => a.IdAnnonce == idAnnonce && a.AnnonceActive == 1);
            TempData["AlertMessageAnnonces"] = "";
            if (annonce1 != null)
            {
                annonce1.AnnonceActive = 0;
                _context.Entry(annonce1).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["AlertMessageAnnonces"] = _stringLocalizer["The ad is deactivated succesfully!"].Value;
            }

            return RedirectToAction("AllAnnoncesAdmin", "Annonces");

            // return RedirectToAction("Index", "Annonces");
        }

        public IActionResult AllAnnoncesAdmin(string nomuser)
        {
            ViewBag.userSession = nomuser;
            var annonces = _context.Annonces.Where(a => a.AnnonceActive == 1).ToList();
            ViewBag.animaux = _context.Animals.ToList();
            ViewBag.images = _context.Images.ToList();
            return View(annonces);
        }


        // afficher toutes les annonces
        public IActionResult AllAnnoncesUser(string nomuser)
        {
            ViewBag.userSession = nomuser;
            var annonces = _context.Annonces.Where(a => a.AnnonceActive == 1).ToList();

            ViewBag.animaux = _context.Animals.Where(a => a.AnimalActif == 1).ToList();
            ViewBag.images = _context.Images.ToList();
            return View(annonces);
        }

        // GET: Annonces/detail/5
        public async Task<IActionResult> DetailAnnonce(int? idAnnonce)
        {
            ViewBag.Phone = "";
            if (idAnnonce == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonces.FindAsync(idAnnonce);
            ViewBag.animal = await _context.Animals.FindAsync(annonce.IdAnimal);
            ViewBag.images = _context.Images.ToList();
            var user = await _context.Utilisateurs.FindAsync(annonce.NomUtilisateur);
           
            if (annonce == null)
            {
                return NotFound();
            }

            return View(annonce);
        }
    }
}
