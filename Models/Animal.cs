using System;
using System.Collections.Generic;

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
        public string NomAnimal { get; set; }
        public string DescriptionAnimal { get; set; }
        public DateTime? DateInscription { get; set; }
        public byte? AnimalActif { get; set; }
        public string Espece { get; set; }
        public string Proprietaire { get; set; }

        public virtual Utilisateur ProprietaireNavigation { get; set; }
        public virtual ICollection<Annonce> Annonces { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
