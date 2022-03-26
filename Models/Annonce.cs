using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class Annonce
    {
        [Display(Name = "No. d'annonce")]
        public int IdAnnonce { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Publié le")]
        public DateTime? DateCreation { get; set; }

        [Required(ErrorMessage = "Champ titre d'annonce obligatoire!")]
        [Display(Name = "Titre d'annonce")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "Champ description obligatoire!")]
        [Display(Name = "Description")]
        public string DescriptionAnnonce { get; set; }

        [Required(ErrorMessage = "Champ ville obligatoire!")]
        [Display(Name = "Ville")]
        public string Ville { get; set; }

        public byte? AnnonceActive { get; set; }

        [Display(Name = "Type d'annonce")]
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
