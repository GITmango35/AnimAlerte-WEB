using System;
using System.Collections.Generic;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class DetailsContact
    {
        public string NomUtilisateurCreateur { get; set; }
        public string NomUtilisateurFavoris { get; set; }
        public DateTime? DateAjout { get; set; }

        public virtual Utilisateur NomUtilisateurCreateurNavigation { get; set; }
        public virtual Utilisateur NomUtilisateurFavorisNavigation { get; set; }
    }
}
