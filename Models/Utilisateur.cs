using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class Utilisateur
    {
        public Utilisateur()
        {
            Animals = new HashSet<Animal>();
            Annonces = new HashSet<Annonce>();
            DetailsContactNomUtilisateurCreateurNavigations = new HashSet<DetailsContact>();
            DetailsContactNomUtilisateurFavorisNavigations = new HashSet<DetailsContact>();
        }

        [StringLength(50)]
        [Display(Name="Username")]
        [Required(ErrorMessage = "Please enter a username!")]
        public string NomUtilisateur { get; set; }

        [StringLength(25)]      
        [Required(ErrorMessage = "Please enter your lastname!")]
        public string Nom { get; set; }

        [StringLength(25)]
        [Required(ErrorMessage = "Please enter your firstname!")]
        [Display(Name = "Last Name")]
        public string Prenom { get; set; }

        [StringLength(25)]
        [Required(ErrorMessage = "Please enter a valid email!")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email not valid!")]
        public string Courriel { get; set; }

        [StringLength(25)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password!")]
        public string MotDePasse { get; set; }

        [StringLength(10)]
        [Display(Name = "Telephone")]
        [Required(ErrorMessage = "Please enter your telephone number!")]
        [RegularExpression(@"[1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]$", ErrorMessage = "Invalid telephone format!")]
        public string NumTel { get; set; }

        [Range(0,1)]
        public byte? UtilisateurActive { get; set; }

        [Range(0, 1)]
        public byte? IsAdmin { get; set; }
        [StringLength(50)]
        public string NomAdminDesactivateur { get; set; }

        public virtual Administrateur NomAdminDesactivateurNavigation { get; set; }
        public virtual Administrateur Administrateur { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual ICollection<Annonce> Annonces { get; set; }
        public virtual ICollection<DetailsContact> DetailsContactNomUtilisateurCreateurNavigations { get; set; }
        public virtual ICollection<DetailsContact> DetailsContactNomUtilisateurFavorisNavigations { get; set; }
    }
}
