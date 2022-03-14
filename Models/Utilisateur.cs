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

        [Key]

        [Display(Name = "Nom d'utilisateur:")]
        [Required(ErrorMessage = "Entrer le nom d'utilisateur")]
        public string NomUtilisateur { get; set; }

        [Display(Name = "Nom:")]
        [Required(ErrorMessage = "Entrer votre nom")]
        public string Nom { get; set; }
        [Display(Name = "Prenom:")]
        [Required(ErrorMessage = "Entrer votre prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Courriel:")]
        [Required(ErrorMessage = "Entrer votre courriel")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Le corriel n'est pas valide.")]
        public string Courriel { get; set; }
        [Display(Name = "Mot de passe:")]
        [Required(ErrorMessage = "Entrer un mot de passe")]
        public string MotDePasse { get; set; }
        [Display(Name = "Numéro de Téléphone:")]
        [RegularExpression(@"^[1-9]\d{2}\d{3}\d{4}", ErrorMessage = "Numero de Telphone n'est pas valide.")]
      
        public string NumTel { get; set; }
        public byte? UtilisateurActive { get; set; }
        public byte? IsAdmin { get; set; }
        public string NomAdminDesactivateur { get; set; }

        public virtual Administrateur NomAdminDesactivateurNavigation { get; set; }
        public virtual Administrateur Administrateur { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual ICollection<Annonce> Annonces { get; set; }
        public virtual ICollection<DetailsContact> DetailsContactNomUtilisateurCreateurNavigations { get; set; }
        public virtual ICollection<DetailsContact> DetailsContactNomUtilisateurFavorisNavigations { get; set; }
    }
}
