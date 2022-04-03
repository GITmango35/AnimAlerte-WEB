using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Nom d'administrateur:")]
        public string NomAdmin { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateCreation { get; set; }
        public virtual Utilisateur NomAdminNavigation { get; set; }
        public virtual ICollection<Annonce> Annonces { get; set; }
        public virtual ICollection<Utilisateur> Utilisateurs { get; set; }
    }
}
