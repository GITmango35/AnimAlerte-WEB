using System;
using System.Collections.Generic;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class Annonce
    {
        public int IdAnnonce { get; set; }
        public DateTime? DateCreation { get; set; }
        public string Titre { get; set; }
        public string DescriptionAnnonce { get; set; }
        public string Ville { get; set; }
        public byte? AnnonceActive { get; set; }
        public string TypeAnnonce { get; set; }
        public int? IdAnimal { get; set; }
        public string NomUtilisateur { get; set; }
        public string NomAdminDesactivateur { get; set; }

        public virtual Animal IdAnimalNavigation { get; set; }
        public virtual Administrateur NomAdminDesactivateurNavigation { get; set; }
        public virtual Utilisateur NomUtilisateurNavigation { get; set; }
    }
}
