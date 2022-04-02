using AnimAlerte.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimAlerte.ViewModels
{
    public class AnimalViewModel
    {
        
        public int IdAnimal { get; set; }

        [Display(Name = "Pet's Name")]
        [Required(ErrorMessage = "Enter the name of your pet !")]
        public string NomAnimal { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Enter the description of your pet !")]
        public string DescriptionAnimal { get; set; }

        [Display(Name = "Registration Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateInscription { get; set; }

        public byte? AnimalActif { get; set; }

        [Display(Name = "Specie")]
        public string Espece { get; set; }

        [Display(Name = "Pet Owner")]
        public string Proprietaire { get; set; }

      
        public Animal Animal { get; set; }

        public Image Image { get; set; }

        [Display(Name = "Photo ")]
        public IFormFile Photo { get; set; }
       
       

       

    }
}
