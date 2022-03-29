using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using AnimAlerte.Models;
using Microsoft.AspNetCore.Http;

namespace AnimAlerte.ViewModels
{
    public class AnnonceViewModel
    {
        [Display(Name = "No. d'annonce")]
        public int IdAnnonce { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date de création")]
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

        public int? IdAnimal { get; set; }
        [Display(Name = "Nom Utilisateur")]
        public string NomUtilisateur { get; set; }

        [Display(Name = "Courriel")]
        public string Courriel { get; set; }
       
        [Display(Name = "Téléphone")]
        public string NumTel { get; set; }

        [Display(Name = "Nom Animal ")]
        public string NomAnimal { get; set; }

        [Display(Name = "Description")]
        public string DescriptionAnimal { get; set; }

        [Display(Name = "Date d'inscription")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateInscription { get; set; }

        public byte? AnimalActif { get; set; }    

        [Display(Name = "Proprietaire")]
        public string Proprietaire { get; set; }

        [Display(Name = "Proprietaire")]
        public virtual Utilisateur ProprietaireNavigation { get; set; }

        public string NomAdminDesactivateur { get; set; }

        public virtual Animal IdAnimalNavigation { get; set; }

        public virtual Administrateur NomAdminDesactivateurNavigation { get; set; }

        [Display(Name = "Propriétaire")]
        public virtual Utilisateur NomUtilisateurNavigation { get; set; }

        public Image Image { get; set; }

        [Display(Name = "Photo ")]
        public IFormFile Photo { get; set; }
    }
}
