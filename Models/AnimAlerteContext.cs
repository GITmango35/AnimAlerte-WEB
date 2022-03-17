using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AnimAlerte.Models
{
    public partial class AnimAlerteContext : DbContext
    {
        public AnimAlerteContext()
        {
        }

        public AnimAlerteContext(DbContextOptions<AnimAlerteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrateur> Administrateurs { get; set; }
        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<Annonce> Annonces { get; set; }
        public virtual DbSet<DetailsContact> DetailsContacts { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }


        //-------METHODES DE MANIPULATION DE LA BD ---------

        // Récuperer tous les animaux d`un utilisateur
        public List<Animal> getAnimalsForUser(string nomuser)
        {
            return Animals.Where(a => a.Proprietaire == nomuser && a.AnimalActif == 1).ToList();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Administrateur>(entity =>
            {

                entity.HasKey(e => e.NomAdmin)
                    .HasName("PK__Administ__47462F6F59BA6D00");

                entity.ToTable("Administrateur");

                entity.Property(e => e.NomAdmin)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nomAdmin");

                entity.Property(e => e.DateCreation)
                    .HasColumnType("date")
                    .HasColumnName("dateCreation")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.NomAdminNavigation)
                    .WithOne(p => p.Administrateur)
                    .HasForeignKey<Administrateur>(d => d.NomAdmin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Constraint__ID");
            });

            modelBuilder.Entity<Animal>(entity =>
            {
              
                entity.HasKey(e => e.IdAnimal)
                    .HasName("PK__Animal__0276B50397F0158D");

                entity.ToTable("Animal");

                entity.Property(e => e.IdAnimal).HasColumnName("idAnimal");

                entity.Property(e => e.AnimalActif)
                    .HasColumnName("animalActif")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DateInscription)
                    .HasColumnType("date")
                    .HasColumnName("dateInscription")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DescriptionAnimal)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descriptionAnimal");

                entity.Property(e => e.Espece)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("espece");

                entity.Property(e => e.NomAnimal)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("nomAnimal");

                entity.Property(e => e.Proprietaire)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("proprietaire");

                entity.HasOne(d => d.ProprietaireNavigation)
                    .WithMany(p => p.Animals)
                    .HasForeignKey(d => d.Proprietaire)
                    .HasConstraintName("fk_nomUtilisateur");
            });

            modelBuilder.Entity<Annonce>(entity =>
            {
                entity.HasKey(e => e.IdAnnonce)
                    .HasName("PK__Annonce__F217B7151A2E7F7A");

                entity.ToTable("Annonce");

                entity.Property(e => e.IdAnnonce).HasColumnName("idAnnonce");

                entity.Property(e => e.AnnonceActive)
                    .HasColumnName("annonceActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DateCreation)
                    .HasColumnType("date")
                    .HasColumnName("dateCreation")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DescriptionAnnonce)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descriptionAnnonce");

                entity.Property(e => e.IdAnimal).HasColumnName("idAnimal");

                entity.Property(e => e.NomAdminDesactivateur)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nomAdminDesactivateur");

                entity.Property(e => e.NomUtilisateur)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nomUtilisateur");

                entity.Property(e => e.Titre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("titre");

                entity.Property(e => e.TypeAnnonce)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("typeAnnonce");

                entity.Property(e => e.Ville)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ville");

                entity.HasOne(d => d.IdAnimalNavigation)
                    .WithMany(p => p.Annonces)
                    .HasForeignKey(d => d.IdAnimal)
                    .HasConstraintName("fk_idAnimal");

                entity.HasOne(d => d.NomAdminDesactivateurNavigation)
                    .WithMany(p => p.Annonces)
                    .HasForeignKey(d => d.NomAdminDesactivateur)
                    .HasConstraintName("fk_nomAdminDesactivateurAnnonce");

                entity.HasOne(d => d.NomUtilisateurNavigation)
                    .WithMany(p => p.Annonces)
                    .HasForeignKey(d => d.NomUtilisateur)
                    .HasConstraintName("fk_nomUtilisateurAnnonce");
            });

            modelBuilder.Entity<DetailsContact>(entity =>
            {
                entity.HasKey(e => new { e.NomUtilisateurCreateur, e.NomUtilisateurFavoris })
                    .HasName("pk_DetailsContact");

                entity.ToTable("DetailsContact");

                entity.Property(e => e.NomUtilisateurCreateur)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nomUtilisateurCreateur");

                entity.Property(e => e.NomUtilisateurFavoris)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nomUtilisateurFavoris");

                entity.Property(e => e.DateAjout)
                    .HasColumnType("date")
                    .HasColumnName("dateAjout")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.NomUtilisateurCreateurNavigation)
                    .WithMany(p => p.DetailsContactNomUtilisateurCreateurNavigations)
                    .HasForeignKey(d => d.NomUtilisateurCreateur)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_nomUtilisateurCreateur");

                entity.HasOne(d => d.NomUtilisateurFavorisNavigation)
                    .WithMany(p => p.DetailsContactNomUtilisateurFavorisNavigations)
                    .HasForeignKey(d => d.NomUtilisateurFavoris)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_nomUtilisateurFavoris");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.IdImage)
                    .HasName("PK__Images__84D649AF9BD87ADB");

                entity.Property(e => e.IdImage).HasColumnName("idImage");

                entity.Property(e => e.IdAnimal).HasColumnName("idAnimal");

                entity.Property(e => e.PathImage)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("pathImage");

                entity.Property(e => e.TitreImage)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("titreImage");

                entity.HasOne(d => d.IdAnimalNavigation)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.IdAnimal)
                    .HasConstraintName("fk_idImageAnimal");
            });

            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasKey(e => e.NomUtilisateur)
                    .HasName("PK__Utilisat__BD52466C45E55527");

                entity.ToTable("Utilisateur");

                entity.Property(e => e.NomUtilisateur)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nomUtilisateur");

                entity.Property(e => e.Courriel)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("courriel");

                entity.Property(e => e.IsAdmin)
                    .HasColumnName("isAdmin")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MotDePasse)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("motDePasse");

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("nom");

                entity.Property(e => e.NomAdminDesactivateur)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nomAdminDesactivateur");

                entity.Property(e => e.NumTel)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("numTel");

                entity.Property(e => e.Prenom)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("prenom");

                entity.Property(e => e.UtilisateurActive)
                    .HasColumnName("utilisateurActive")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.NomAdminDesactivateurNavigation)
                    .WithMany(p => p.Utilisateurs)
                    .HasForeignKey(d => d.NomAdminDesactivateur)
                    .HasConstraintName("fk_nomAdminDesactivateur");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        internal object getAnimalsForUser(object usersession)
        {
            throw new NotImplementedException();
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
