using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimAlerte.Models;
using Microsoft.AspNetCore.Hosting;
using AnimAlerte.ViewModels;
using AnimAlerte.Controllers;
using System.IO;

namespace AnimAlerte.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly AnimAlerteContext _context;
        private readonly IWebHostEnvironment hosting;

        public AnimalsController(AnimAlerteContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            this.hosting = hosting;
        }

        public IActionResult Index()
        {
            var animaux = _context.getAnimalsForUser(UtilisateursController.usersession).ToList();
            ViewBag.images = _context.Images.ToList();
            return View(animaux);
        }

        public IActionResult MesAnimaux()
        {
            var animaux = _context.getAnimalsForUser(UtilisateursController.usersession).ToList();
            ViewBag.images = _context.Images.ToList();
            return View(animaux);
           
        }


        // GET: Animals2/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.ProprietaireNavigation)
                .FirstOrDefaultAsync(m => m.IdAnimal == id);
            var image = await _context.Images
                .Include(a => a.IdAnimalNavigation)
                .FirstOrDefaultAsync(m => m.IdAnimal == id);

            var model = new ImageAnimalModifViewModel()
            {
                IdAnimal = animal.IdAnimal,
                NomAnimal = animal.NomAnimal,
                DescriptionAnimal = animal.DescriptionAnimal,
                DateInscription = animal.DateInscription,
                AnimalActif = 1,
                Espece = animal.Espece,
                Proprietaire = animal.Proprietaire,
                PhotoPath = image.PathImage
            };

            if (animal == null)
            {
                return NotFound();
            }

            return View(model);
        }

        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var animal = await _context.Animals
        //        .Include(a => a.ProprietaireNavigation)
        //        .FirstOrDefaultAsync(m => m.IdAnimal == id);
        //    if (animal == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(animal);
        //}


        // GET: Animals/Create
        public IActionResult AjoutAnimal()
        {

            ViewBag.us = UtilisateursController.usersession;
            var model = new ImageAnimalViewModel
            {

            };
            return View(model);
        }

        // POST: Animals/Create
        [HttpPost]
        public IActionResult AjoutAnimal(ImageAnimalViewModel model)
        {
            string proprietaire = UtilisateursController.usersession;
            Animal animal = new Animal();
            Image image = new Image();

            if (ModelState.IsValid)
            {
                string fileName = null;

                if (model.Photo != null)
                {

                    string extFile = Path.GetExtension(model.Photo.FileName);
                    string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                    fileName = Guid.NewGuid() + "_" + model.Photo.FileName;
                    string fullPath = Path.Combine(uploads, fileName);
                    model.Photo.CopyTo(new FileStream(fullPath, FileMode.Create));

                }

                animal.NomAnimal = model.NomAnimal;
                animal.DescriptionAnimal = model.DescriptionAnimal;
                animal.DateInscription = DateTime.Today;
                animal.AnimalActif = 1;
                animal.Espece = model.Espece;
                animal.Proprietaire = proprietaire;

                _context.Animals.Add(animal);
                _context.SaveChanges();

                image.TitreImage = model.NomAnimal;
                if (fileName != null)
                {
                    image.PathImage = fileName;
                }

                image.IdAnimal = animal.IdAnimal;
                _context.Images.Add(image);
                _context.SaveChanges();

                return RedirectToAction(nameof(MesAnimaux));
            }

            return View(model);
        }


        // GET: Animals/ModifierAnimal

        public async Task<IActionResult> ModifierAnimal(int? idAnimal)
        {
            if (idAnimal == null)
            {
                return NotFound();
            }

            Animal animal = await _context.Animals.SingleOrDefaultAsync(ani => ani.IdAnimal == idAnimal);
            Image image = await _context.Images.SingleOrDefaultAsync(i => i.IdAnimal == idAnimal);

            ImageAnimalModifViewModel model = new ImageAnimalModifViewModel()
            {
                IdAnimal = animal.IdAnimal,
                NomAnimal = animal.NomAnimal,
                DescriptionAnimal = animal.DescriptionAnimal,
                DateInscription = animal.DateInscription,
                AnimalActif = animal.AnimalActif,
                Espece = animal.Espece,
                Proprietaire = animal.Proprietaire,
                PhotoPath = image.PathImage
            };

            if (animal == null)
            {
                return NotFound();
            }

            return View(model);
        }


        // POST: Animals/ModifierAnimal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifierAnimal(ImageAnimalModifViewModel model)
        {


            if (ModelState.IsValid)
            {
                Animal animal = await _context.Animals.SingleOrDefaultAsync(ani => ani.IdAnimal == model.IdAnimal);
                animal.NomAnimal = model.NomAnimal;
                animal.DescriptionAnimal = model.DescriptionAnimal;
                animal.DateInscription = model.DateInscription;
                animal.AnimalActif = 1;
                animal.Espece = model.Espece;
                animal.Proprietaire = model.Proprietaire;

                Image image = await _context.Images.SingleOrDefaultAsync(i => i.IdAnimal == model.IdAnimal);
                string fileName = null;

                if (model.Photo != null)
                {
                    if (model.PhotoPath != null)
                    {
                        string filePath = Path.Combine(hosting.WebRootPath, "uploads", model.PhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    string extFile = Path.GetExtension(model.Photo.FileName);

                    string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                    fileName = Guid.NewGuid() + "_" + model.Photo.FileName;
                    string fullPath = Path.Combine(uploads, fileName);
                    model.Photo.CopyTo(new FileStream(fullPath, FileMode.Create));
                    image.TitreImage = model.NomAnimal;
                    image.PathImage = fileName;
                    image.IdAnimal = model.IdAnimal;
                }

                var animalmodif = _context.Animals.Attach(animal);
                animalmodif.State = EntityState.Modified;
                _context.SaveChanges();


                var imageModif = _context.Images.Attach(image);
                imageModif.State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction(nameof(MesAnimaux));
            }
            return View(model);
        }



        //public IActionResult ModifierAnimal(int idAnimal)
        //{
        //    Animal animal = _context.Animals.SingleOrDefault(ani => ani.IdAnimal == idAnimal);
        //    Image image = _context.Images.SingleOrDefault(i => i.IdAnimal == idAnimal);

        //    ModifImageAnimalViewModel model = new ModifImageAnimalViewModel()
        //    {
        //        IdAnimal = animal.IdAnimal,
        //        NomAnimal = animal.NomAnimal,
        //        DescriptionAnimal = animal.DescriptionAnimal,
        //        DateInscription = animal.DateInscription,
        //        AnimalActif = animal.AnimalActif,
        //        Espece = animal.Espece,
        //        Proprietaire = animal.Proprietaire,
        //        PhotoPath = image.PathImage
        //    };

        //    return View(model);
        //}


        //// POST: Animals/ModifierAnimal
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ModifierAnimal(ModifImageAnimalViewModel model)
        //{


        //    if (ModelState.IsValid)
        //    {
        //        Animal animal = _context.Animals.SingleOrDefault(ani => ani.IdAnimal == model.IdAnimal);
        //        animal.NomAnimal = model.NomAnimal;
        //        animal.DescriptionAnimal = model.DescriptionAnimal;
        //        animal.DateInscription = model.DateInscription;
        //        animal.AnimalActif = 1;
        //        animal.Espece = model.Espece;
        //        animal.Proprietaire = model.Proprietaire;

        //        Image image = _context.Images.SingleOrDefault(i => i.IdAnimal == model.IdAnimal);
        //        string fileName = null;

        //        if (model.Photo != null)
        //        {
        //            string extFile = Path.GetExtension(model.Photo.FileName);

        //            string uploads = Path.Combine(hosting.WebRootPath, "uploads");
        //            fileName = Guid.NewGuid() + "_" + model.Photo.FileName;
        //            string fullPath = Path.Combine(uploads, fileName);
        //            model.Photo.CopyTo(new FileStream(fullPath, FileMode.Create));
        //            image.TitreImage = model.NomAnimal;
        //            image.PathImage = fileName;
        //            image.IdAnimal = model.IdAnimal;
        //        }

        //        var animalmodif = _context.Animals.Attach(animal);
        //        animalmodif.State = EntityState.Modified;
        //        _context.SaveChanges();


        //        var imageModif = _context.Images.Attach(image);
        //        imageModif.State = EntityState.Modified;
        //        _context.SaveChanges();

        //        return RedirectToAction(nameof(MesAnimaux));
        //    }
        //    return View(model);
        //}


        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? idAnimal)
        {
            if (idAnimal == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals.FindAsync(idAnimal);
            ViewBag.image = await _context.Images.FindAsync(animal.IdAnimal);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int idAnimal)
        {
            var animal = _context.Animals.Find(idAnimal);
            animal.AnimalActif = 0;
            var animalModif = _context.Animals.Attach(animal);
            animalModif.State = EntityState.Modified;
            
            _context.SaveChanges();

            return RedirectToAction(nameof(MesAnimaux));
        }

    }
}
