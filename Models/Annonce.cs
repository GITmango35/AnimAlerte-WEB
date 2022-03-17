using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class Annonce
    {
        [Display(Name = "Numéro d'annonce")]
        public int IdAnnonce { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]

        [Display(Name = "Date création")]
        public DateTime? DateCreation { get; set; }

        [Required]
        [Display(Name = "Titre Annonce")]
        public string Titre { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string DescriptionAnnonce { get; set; }
        [Required]
        [Display(Name = "Ville")]
        public string Ville { get; set; }
        public byte? AnnonceActive { get; set; }
        [Required]
        [Display(Name = "Type Annonce")]
        public string TypeAnnonce { get; set; }
        
        [Required(ErrorMessage = "SVP, Veuillez créer un nouvel animal!")]
        public int? IdAnimal { get; set; }
        [Display(Name = "Nom Utilisateur")]
        public string NomUtilisateur { get; set; }
        public string NomAdminDesactivateur { get; set; }

        public virtual Animal IdAnimalNavigation { get; set; }
        public virtual Administrateur NomAdminDesactivateurNavigation { get; set; }
        public virtual Utilisateur NomUtilisateurNavigation { get; set; }
    }
}
