using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class Annonce
    {
        [Display(Name = "Ad No.")]
        public int IdAnnonce { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Publish Date")]
        public DateTime? DateCreation { get; set; }

        [Required(ErrorMessage = "Title field is required !")]
        [Display(Name = "Ad Title")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "Description field is required !")]
        [Display(Name = "Description")]
        public string DescriptionAnnonce { get; set; }

        [Required(ErrorMessage = "City field is required !")]
        [Display(Name = "City")]
        public string Ville { get; set; }

        public byte? AnnonceActive { get; set; }

        [Display(Name = "Ad type")]
        [Required(ErrorMessage = "Please choose the ad type !")]
        public string TypeAnnonce { get; set; }
        
        [Required(ErrorMessage = "SVP, Veuillez ajouter un nouvel animal!")]
        public int? IdAnimal { get; set; }

        [Display(Name = "Nom Utilisateur")]
        public string NomUtilisateur { get; set; }
        public string NomAdminDesactivateur { get; set; }

        public virtual Animal IdAnimalNavigation { get; set; }

        public virtual Administrateur NomAdminDesactivateurNavigation { get; set; }

        [Display(Name = "Propriétaire")]
        public virtual Utilisateur NomUtilisateurNavigation { get; set; }
    }
}
