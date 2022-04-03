using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class DetailsContact
    {

        [Display(Name="Utilisateur")]
        public string NomUtilisateurCreateur { get; set; }

        [Display(Name="Contact Favoris")]
        public string NomUtilisateurFavoris { get; set; }

        [Display(Name="Date d'ajout")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateAjout { get; set; }

        public byte? isFavoris { get; set; }
        
        public virtual Utilisateur NomUtilisateurCreateurNavigation { get; set; }
        [Display(Name = "Contact Favoris")]
        public virtual Utilisateur NomUtilisateurFavorisNavigation { get; set; }
    }
}
