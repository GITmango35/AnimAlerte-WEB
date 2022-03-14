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
        [Display(Name="Nom d'utilisateur")]
        [Required(ErrorMessage = "SVP ENTREZ UN NOM D'UTILISATEUR")]
        public string NomUtilisateur { get; set; }
        [StringLength(25)]
        [Required(ErrorMessage = "SVP ENTREZ VOTRE NOM")]
        public string Nom { get; set; }
        [StringLength(25)]
        [Required(ErrorMessage = "SVP ENTREZ VOTRE PRENOM")]
        [Display(Name = "Prénon")]
        public string Prenom { get; set; }
        [StringLength(25)]
        [Required(ErrorMessage = "SVP ENTREZ UN COURRIEL VALIDE")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "COURRIEL NON VALIDE")]
        public string Courriel { get; set; }
        [StringLength(25)]
        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "SVP ENTREZ UN MOT DE PASSE")]
        public string MotDePasse { get; set; }
        [StringLength(10)]
        [Display(Name = "Téléphone")]
        [Required(ErrorMessage = "SVP ENTREZ VOTRE TELEPHONE")]
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
