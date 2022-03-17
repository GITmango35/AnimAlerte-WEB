using AnimAlerte.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimAlerte.ViewModels
{
    public class ImageAnimalViewModel
    {
        
        public int IdAnimal { get; set; }

        [Display(Name = "Nom Animal ")]
        [Required(ErrorMessage = "Entrer le nom de votre animal")]
        public string NomAnimal { get; set; }

        [Display(Name = "Description")]
        public string DescriptionAnimal { get; set; }

        [Display(Name = "Date d'inscription")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateInscription { get; set; }

        public byte? AnimalActif { get; set; }

        [Display(Name = "Espéce")]
        public string Espece { get; set; }

        [Display(Name = "Proprietaire")]
        public string Proprietaire { get; set; }

      
        public Animal Animal { get; set; }

        public Image Image { get; set; }


        [Display(Name = "Photo ")]
        public IFormFile Photo { get; set; }
       
       

       

    }
}
