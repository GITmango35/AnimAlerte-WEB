using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class Animal
    {
        public Animal()
        {
            Annonces = new HashSet<Annonce>();
            Images = new HashSet<Image>();
        }
       
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
        public string Proprietaire { get; set; }

        [Display(Name = "Proprietaire")]
        public virtual Utilisateur ProprietaireNavigation { get; set; }
        public virtual ICollection<Annonce> Annonces { get; set; }
        public virtual ICollection<Image> Images { get; set; }

    }
}
