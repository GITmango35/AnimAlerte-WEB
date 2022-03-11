using System;
using System.Collections.Generic;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class Administrateur
    {
        public Administrateur()
        {
            Annonces = new HashSet<Annonce>();
            Utilisateurs = new HashSet<Utilisateur>();
        }

        public string NomAdmin { get; set; }
        public DateTime? DateCreation { get; set; }

        public virtual Utilisateur NomAdminNavigation { get; set; }
        public virtual ICollection<Annonce> Annonces { get; set; }
        public virtual ICollection<Utilisateur> Utilisateurs { get; set; }
    }
}
