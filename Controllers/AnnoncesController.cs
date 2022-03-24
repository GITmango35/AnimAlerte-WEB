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
        // Index retour la liste des annonces, une barre de recherche par ville, Trier par Date de création, filtrer par type d'annonce (perdu ou trouvé)
        public async Task<IActionResult> Index(string nomuser, string sortOrder, string searchString, string annoncePerdu, string annonceTrouve)
        {
            ViewData["IdAnnonceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            ViewData["LostAnimalFilter"] = annoncePerdu == "perdu" ? "trouve" : "perdu";
            ViewData["FoundAnimalFilter"] = annonceTrouve == "trouve" ? "perdu" : "trouve";

            ViewBag.userSession = nomuser;
            ViewBag.allAnnonces = _context.Annonces.Where(a => a.NomUtilisateur == session.GetString("NomUtilisateur") && a.AnnonceActive == 1).ToList();
            ViewBag.animaux = _context.Animals.ToList();
            ViewBag.images = _context.Images.ToList();
            var annonces = from a in _context.Annonces
                           select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                annonces = annonces.Where(a => a.Ville.ToUpper().Contains(searchString.ToUpper()) && a.AnnonceActive == 1);
            }

            if (!String.IsNullOrEmpty(annoncePerdu))
            {
                annonces = annonces.Where(a => a.TypeAnnonce == "perdu" && a.AnnonceActive == 1);
            }

            if (!String.IsNullOrEmpty(annonceTrouve))
            {
                annonces = annonces.Where(a => a.TypeAnnonce == "trouve" && a.AnnonceActive == 1);
            }

            switch (sortOrder)
            {
                case "id_desc":
                    annonces = annonces.OrderByDescending(a => a.IdAnnonce);
                    break;
                case "Date":
                    annonces = annonces.OrderBy(a => a.DateCreation);
                    break;
                case "date_desc":
                    annonces = annonces.OrderByDescending(a => a.DateCreation);
                    break;
                default:
                    annonces = annonces.OrderBy(a => a.IdAnnonce);
                    break;
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
        //public IActionResult Details(int? id, string nomuser)
        //{
           
public async Task<IActionResult> Details2(int? id)
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


            /* ViewBag.userSession = nomuser;
             var annonces = _context.Annonces.Find(id);
             ViewBag.animaux = _context.Animals.ToList();
             ViewBag.images = _context.Images.ToList();
             return View(annonces);*/



            /*   if (id == null)
               {
                   return NotFound();
               }

               var annonce = _context.Annonces.Find(id);

               *//* var annonce = _context.Annonces

                    .FirstOrDefault(m => m.IdAnnonce == id);*//*

               var animal = _context.Animals

                 .FirstOrDefault(m => m.IdAnimal == id);

               var image = _context.Images

                  .FirstOrDefault(m => m.IdImage == id);

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
                   Espece = animal.Espece,
                   Proprietaire = animal.Proprietaire,
                   PhotoPath = image.PathImage
               };

               if (model == null)
               {
                   return NotFound();
               }

               return View(model);
        }*/

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

        //L'administrateur peut rechercher une annonce afin de la desactiver
        public ActionResult RechercheAnnonce()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RechercheAnnonce(int idAnnonce)
        {            
            var annonce = _context.Annonces.SingleOrDefault(a => a.IdAnnonce == idAnnonce && a.AnnonceActive == 1);
            ViewBag.animals = _context.Animals.ToList();
            return View(annonce); //recuperer les infos d'annonce
        }



        //la désactivation d'annonce par un admin
        public ActionResult DesactiverAnnonce(int idAnnonce)
        {
            var annonce = _context.Annonces.SingleOrDefault(a => a.IdAnnonce == idAnnonce && a.AnnonceActive == 1);
            ViewBag.admin = UtilisateursController.usersession;
            return View(annonce);
        }



        [HttpPost]
        public ActionResult DesactiverAnnonce(int idAnnonce, Annonce annonce)
        {
            var annonce1 = _context.Annonces.SingleOrDefault(a => a.IdAnnonce == idAnnonce && a.AnnonceActive == 1);
            if (annonce1 != null)
            {
                annonce1.AnnonceActive = 0;
                _context.Entry(annonce1).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return RedirectToAction("AllAnnoncesAdmin", "Annonces");

           // return RedirectToAction("Index", "Annonces");
        }
        //public IActionResult AllAnnoncesAdmin(string nomuser)
        //{
        // //Affichage de toute les annonces pour admin
        //            ViewBag.userSession = nomuser;
        //    ViewBag.userSession = nomuser;
        //    var annonces = _context.Annonces.ToList();
        //    ViewBag.animaux = _context.Animals.ToList();
        //    ViewBag.images = _context.Images.ToList();
        //    return View(annonces);
        //    {

        //// afficher toutes les annonces
        //public IActionResult AllAnnonces(string nomuser)
        //// Affichage de toutes les annonces pour utilisateur
        //    ViewBag.userSession = nomuser;
        //{
        //    ViewBag.userSession = nomuser;
        //    var annonces = _context.Annonces.ToList();
        //    ViewBag.animaux = _context.Animals.ToList();
        //    ViewBag.images = _context.Images.ToList();
        //    return View(annonces);
        //Affichage de toute les annonces pour admin
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
            var annonces = _context.Annonces.ToList();
            ViewBag.animaux = _context.Animals.ToList();
            ViewBag.images = _context.Images.ToList();
            return View(annonces);
        }


    }
}
