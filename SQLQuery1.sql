
USE animalerte4;




CREATE TABLE Administrateur(
  nomAdmin VARCHAR(50) PRIMARY KEY,
  dateCreation DATE DEFAULT (GETDATE())
    
);

CREATE TABLE Utilisateur(
	nomUtilisateur VARCHAR(50) PRIMARY KEY,
	nom VARCHAR(25) NOT NULL,
	prenom VARCHAR(25) NOT NULL,
	courriel VARCHAR(25) NOT NULL,
	motDePasse VARCHAR(25) NOT NULL,
	numTel VARCHAR(10) NOT NULL,
	utilisateurActive TINYINT DEFAULT 1,
  	isAdmin TINYINT DEFAULT 0,
  	nomAdminDesactivateur VARCHAR(50) NULL,
    	CONSTRAINT fk_nomAdminDesactivateur FOREIGN KEY (nomAdminDesactivateur) REFERENCES Administrateur(nomAdmin)
);


CREATE TABLE Animal (
    idAnimal INT IDENTITY(1,1)PRIMARY KEY,
    nomAnimal VARCHAR(25) NOT NULL,
    descriptionAnimal VARCHAR(255) NOT NULL,
    dateInscription DATE DEFAULT GETDATE(),
    animalActif TINYINT DEFAULT 1,
    espece VARCHAR(25),
      CONSTRAINT CHK_Espece CHECK ( espece ='Chien' OR espece ='Chat' OR espece ='Autres'),
    proprietaire varchar(50),
		  CONSTRAINT fk_nomUtilisateur FOREIGN KEY (proprietaire) REFERENCES Utilisateur(nomUtilisateur)
);

CREATE TABLE Images (
  idImage INT IDENTITY(1,1) PRIMARY KEY,
  titreImage VARCHAR(25),
  pathImage VARCHAR(255),
  idAnimal INT,
    CONSTRAINT fk_idImageAnimal FOREIGN KEY (idAnimal) REFERENCES Animal(idAnimal)
);

CREATE TABLE Annonce(
    idAnnonce INT identity(1,1) PRIMARY KEY ,
    dateCreation DATE DEFAULT (GETDATE()),
    titre VARCHAR(50) NOT NULL,
    descriptionAnnonce VARCHAR(255),
    ville VARCHAR(25) NOT NULL,
    annonceActive tinyint DEFAULT 1,
    typeAnnonce VARCHAR(25) NOT NULL,
	    CONSTRAINT CHK_Type CHECK ( typeAnnonce ='Perdu' OR typeAnnonce ='Trouve'),
    idAnimal INT,
		  CONSTRAINT fk_idAnimal FOREIGN KEY (idAnimal) REFERENCES Animal(idAnimal),
    nomUtilisateur VARCHAR(50),
		  CONSTRAINT fk_nomUtilisateurAnnonce FOREIGN KEY(nomUtilisateur) REFERENCES Utilisateur(nomUtilisateur),
    nomAdminDesactivateur VARCHAR(50),
      CONSTRAINT fk_nomAdminDesactivateurAnnonce FOREIGN KEY (nomAdminDesactivateur) REFERENCES Administrateur(nomAdmin)
);


CREATE TABLE DetailsContact(
    nomUtilisateurCreateur VARCHAR(50),
    nomUtilisateurFavoris VARCHAR(50),
        CONSTRAINT pk_DetailsContact PRIMARY KEY (nomUtilisateurCreateur, nomUtilisateurFavoris),
    dateAjout DATE DEFAULT (GETDATE()),
        CONSTRAINT fk_nomUtilisateurCreateur FOREIGN KEY(nomUtilisateurCreateur) REFERENCES Utilisateur(nomUtilisateur),
		    CONSTRAINT fk_nomUtilisateurFavoris FOREIGN KEY(nomUtilisateurFavoris) REFERENCES Utilisateur(nomUtilisateur)
);


ALTER TABLE Administrateur ADD CONSTRAINT FK__Constraint__ID FOREIGN KEY (nomAdmin) REFERENCES Utilisateur;


