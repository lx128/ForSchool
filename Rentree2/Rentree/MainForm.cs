/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 08/09/2010
 * Heure: 21:12
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using System.IO;
using System.Text;
using System.IO.MemoryMappedFiles;

namespace Rentree
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		void Button1Click(object sender, EventArgs e)//Elèves
		{
			string line,str, str2, slogin="", slogin2,
				smdp="", sclasse, sprenom="", snom="",srespNom1="",srespPrenom1="",
				srespID1="",srespNom2="",srespPrenom2="",srespID2="",
				sbirth,
				snomsco="", sprenomsco="",// sclassesco="", sinesco="",
				snomrespsco1,sprenomrespsco1,
				snomrespsco2,sprenomrespsco2,
				//srespentnom="",srespentprenom="",//srespentID1="",
				//srespentnom2="",srespentprenom2="",//srespentID2="",
				scategorie="",srespMDP1="",srespMDP2="",setablissement="",suid="",scivilite=""
				,saverespID1,saverespID2

				/*sresp11="", sresp12="",sresp13="", sresp14="",sresp15="",
				sresp16="", sresp17="",sresp18="", sresp19="",sresp110="",
				sresp111="", sresp112="",sresp113="",sresp114="",*/

				/*sresp21="", sresp22="",sresp23="", sresp24="",sresp25="",
				sresp26="", sresp27="",sresp28="", sresp29="",sresp210="",
				sresp211="", sresp212="",sresp213="",sresp214=""*/;

			byte comp=0, comp3, nom=0, prenom=0, id=0,
				mdp=0, classe=0, categorie=0,respNom1=0,respPrenom1=0,
				respID1=0,respNom2=0,respPrenom2=0,respID2=0,//eleveID3=0,
				birth=0, nomsco=0, prenomsco=0,classesco=0,inesco=0,
				//respentnom=0,respentprenom=0,respentID=0,respentMDP=0,
				etablissement=0,uid=0,civilite=0,

				//nomrespsco=0, prenomrespsco=0,resp13=0, resp14=0,resp15=0,
				nomrespsco1=0,prenomrespsco1=0,
				nomrespsco2=0,prenomrespsco2=0
//				resp16=0, resp17=0,resp18=0, resp19=0,resp110=0,
//				resp111=0, resp112=0,resp113=0,resp114=0,*/

				/*resp21=0, resp22=0,resp23=0, resp24=0,resp25=0,
				resp26=0, resp27=0,resp28=0, resp29=0,resp210=0,
				resp211=0, resp212=0,resp213=0,resp214=0*/;

			byte[] an_array= new byte[64];
			int comp2=0, ret=0;

			if (!File.Exists("Delta Elv.txt"))
			    {
					FileStream cr = File.Create("Delta Elv.txt");
					cr.Close();
			    }
			StreamReader deltar;
			StreamWriter deltaw;

			StreamReader elv;
			try{
				elv = new StreamReader("ent.csv");//Fichier ENT
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message+"\nCe fichier est une extraction de l'ENT Eleves+Parents");
				return;
			}
			StreamReader respent;
			try{
				respent = new StreamReader("ent.csv");//Fichier responsable ENT
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message+"\nCe fichier est une extraction de l'ENT Eleves+Parents");
				return;
			}
			if (!File.Exists("siecle.csv"))
			    {
					MessageBox.Show("Fichier siecle.csv introuvable.\nCe fichier est une extraction siecle (anciennement sconet)) contenant les champs:\n" +
				                "Nom	Prénom 1	Date de naissance	Code Structure	Nom resp. légal1	Prénom resp. légal1	Nom resp. légal2	Prénom resp. légal2\n\n"
				               +"Une extraction Eleve complete avec ; pour séparateur fonctionne aussi");
					return;
				}
			FonctionFichierBinaireCLR siecle= new FonctionFichierBinaireCLR("siecle.csv","r");

			if (!Directory.Exists("eleve"))
			    {
			    	Directory.CreateDirectory("eleve");
			    }
			File.Delete("eleve/Liste Compte Eleves.csv");
			File.Delete("eleve/Pronote.txt");
			File.Delete("eleve/AdeleEnt.csv");
			//File.Delete("eleve/Script.cmd");
			//FonctionFichierBinaireCLR script = new FonctionFichierBinaireCLR("eleve/Script.cmd");//Script
			FonctionFichierBinaireCLR lelv = new FonctionFichierBinaireCLR("eleve/Liste Compte Eleves.csv","W");//Pour Adele
			FonctionFichierBinaireCLR pronote = new FonctionFichierBinaireCLR("eleve/Pronote.txt","w");//Pronote
			StreamWriter adele_ent = new StreamWriter("eleve/AdeleEnt.csv");//Liste de codes pour synchro ENT/Adele

			elv.ReadLine();
			line=elv.ReadLine();
			//test du séparateur
			if (line.IndexOf(",")==-1)
			{
				//script.Ferme();
				siecle.Ferme();
				adele_ent.Close();
				pronote.Ferme();
				lelv.Ferme();
				respent.Close();
				elv.Close();
				MessageBox.Show("Le fichier ent.csv doit avoir , pour séparateur de colone.\n");
				return;
			}
			richTextBox1.Text="";
			label1.Text="";
			//Etape 1
			for(comp=0;-1!=FonctionCLR.element(line,comp, out str, ',');comp++)
			{
//Nom,Prénom,Identifiant,Mot de passe,Classe
				switch (str)
				{
				case "Etablissement":
					etablissement=comp;
					ret=ret+1;
					break;
				case "UID":
					uid=comp;
					ret=ret+1;
					break;
				case "Civilité":
					civilite=comp;
					ret=ret+1;
					break;
				case "Nom":
					nom=comp;
					ret=ret+1;
					break;
				case "Prénom":
				case "Pr�nom":
				case "Prenom":
					prenom=comp;
					ret=ret+1;
					break;
				case "Identifiant":
					id=comp;
					ret=ret+1;
					break;
				case "Mot de passe":
					mdp=comp;
					ret=ret+1;
					break;
				case "Classe":
					classe=comp;
					ret=ret+1;
					break;
				case "Catégorie":
					categorie=comp;
					ret=ret+1;
					break;

				case "Nom élève/parent 1":
					respNom1=comp;
					ret=ret+1;
					break;
				case "Prénom élève/parent 1":
					respPrenom1=comp;
					ret=ret+1;
					break;
				case "Identifiant élève/parent 1":
					respID1=comp;
					ret=ret+1;
					break;


				case "Nom élève/parent 2":
					respNom2=comp;
					ret=ret+1;
					break;
				case "Prénom élève/parent 2":
					respPrenom2=comp;
					ret=ret+1;
					break;
				case "Identifiant élève/parent 2":
					respID2=comp;
					ret=ret+1;
					break;

/*				case "Identifiant élève/parent 3":
					eleveID3=comp;
					ret=ret+1;
					break;
*/				}
			}
			elv.Close();
			if(ret!=15)
			{
				//script.Ferme();
				siecle.Ferme();
				adele_ent.Close();
				pronote.Ferme();
				lelv.Ferme();
				respent.Close();
				MessageBox.Show("Le fichier ent.csv ne contient pas tout les champs ou la deuxieme ligne ne les présente pas.\nCe fichier est une extraction ENT eleves+parents");
				return;
			}

			//siecle doit avoir ";" car certaine champ adresse contiennent déjà ","
			ret=0;
			line=siecle.LireLigne();
			//test du séparateur
			if (line.IndexOf(";")==-1)
			{
				respent.Close();
	
				//script.Ferme();
				siecle.Ferme();
				adele_ent.Close();
				pronote.Ferme();
				lelv.Ferme();
				respent.Close();
				MessageBox.Show("Le fichier siecle.csv doit avoir ; pour séparateur de colone.\n");
				return;
			}
			for(comp=0;-1!=FonctionCLR.element(line,comp, out str, ';');comp++)
			{
				switch (str)
				{
				case "Date Naissance":
				case "Date de naissance":
				case "NE(E) LE":
					birth=comp;
					ret=ret+1;
					break;
				case "Nom":
				case "NOM":
					nomsco=comp;
					ret=ret+1;
					break;
				case "Prénom":
				case "PRENOM":
				case "Pr�nom":
				case "Prenom":
				case "Prénom 1":
				case "PRENOM 1":
				case "Pr�nom 1":
				case "Prenom 1":
					prenomsco=comp;
					ret=ret+1;
					break;



				case "Nom Resp. Lég. 1":
					nomrespsco1=comp;
					ret=ret+1;
					break;
				case "Prénom Resp. Lég. 1":
					prenomrespsco1=comp;
					ret=ret+1;
					break;
/*				case "Ligne 1 Adresse légal1":
					resp13=comp;
					ret=ret+1;
					break;
				case "Ligne 2 Adresse légal1":
					resp14=comp;
					ret=ret+1;
					break;
				case "Civilité resp. légal1":
					resp15=comp;
					ret=ret+1;
					break;
				case "Commune resp. légal1":
					resp16=comp;
					ret=ret+1;
					break;
				case "Code postal resp. légal1":
					resp17=comp;
					ret=ret+1;
					break;
				case "Tel maison resp. légal1":
					resp18=comp;
					ret=ret+1;
					break;
				case "Tel travail resp. légal1":
					resp19=comp;
					ret=ret+1;
					break;
				case "Tel mobile resp. légal1":
					resp110=comp;
					ret=ret+1;
					break;
				case "Courriel resp. légal1":
					resp111=comp;
					ret=ret+1;
					break;
				case "Com. adresse légal1":
					resp112=comp;
					ret=ret+1;
					break;
				case "Autorise SMS légal1":
					resp113=comp;
					ret=ret+1;
					break;
				case "Ligne 3 Adresse légal1":
					resp114=comp;
					ret=ret+1;
					break;
*/

				case "Nom Resp. Lég. 2":
					nomrespsco2=comp;
					ret=ret+1;
					break;
				case "Prénom Resp. Lég. 2":
					prenomrespsco2=comp;
					ret=ret+1;
					break;
/*				case "Ligne 1 Adresse Légal2":
					resp23=comp;
					ret=ret+1;
					break;
				case "Ligne 2 Adresse Légal2":
					resp24=comp;
					ret=ret+1;
					break;
				case "Civilité resp. Légal2":
					resp25=comp;
					ret=ret+1;
					break;
				case "Commune resp. Légal2":
					resp26=comp;
					ret=ret+1;
					break;
				case "Code postal resp. Légal2":
					resp27=comp;
					ret=ret+1;
					break;
				case "Tel maison resp. Légal2":
					resp28=comp;
					ret=ret+1;
					break;
				case "Tel travail resp. Légal2":
					resp29=comp;
					ret=ret+1;
					break;
				case "Tel mobile resp. Légal2":
					resp210=comp;
					ret=ret+1;
					break;
				case "Courriel resp. Légal2":
					resp211=comp;
					ret=ret+1;
					break;
				case "Com. adresse Légal2":
					resp212=comp;
					ret=ret+1;
					break;
				case "Autorise SMS Légal2":
					resp213=comp;
					ret=ret+1;
					break;
				case "Ligne 3 Adresse Légal2":
					resp214=comp;
					ret=ret+1;
					break;
*/


				case "Code Structure":
					classesco=comp;
					ret=ret+1;
					break;
				case "Id National":
					inesco=comp;
					ret=ret+1;
					break;
				}
			}
			if(ret<7)
			{
				elv.Close();

				//script.Ferme();
				siecle.Ferme();
				adele_ent.Close();
				pronote.Ferme();
				lelv.Ferme();
				respent.Close();
				MessageBox.Show("Le fichier siecle.csv ne contient pas tout les champs ou la deuxième ligne ne les présente pas.\n" +
				                "Faire une extraction Eleve avec adresses");
				return;
			}

/*			ret=0;
			line=respent.ReadLine();
			line=respent.ReadLine();
			//test du séparateur
			if (line.IndexOf(",")==-1)
			{
				respent.Close();
	
				//script.Ferme();
				siecle.Ferme();
				adele_ent.Close();
				pronote.Ferme();
				lelv.Ferme();
				MessageBox.Show("Le fichier parent.csv doit avoir , pour séparateur de colone.\n");
				return;
			}

			for(comp=0;-1!=FonctionCLR.element(line,comp, out str, ',');comp++)
			{
				switch (str)
				{
				case "Nom":
					respentnom=comp;
					ret=ret+1;
					break;
				case "Prénom":
				case "Pr�nom":
				case "Prenom":
					respentprenom=comp;
					ret=ret+1;
					break;
				case "Identifiant":
					respentID=comp;
					ret=ret+1;
					break;
				case "Mot de passe":
					respentMDP=comp;
					ret=ret+1;
					break;
				}
			}
			if(ret<4)
			{
			respent.Close();

			//script.Ferme();
			siecle.Ferme();
			adele_ent.Close();
			pronote.Ferme();
			lelv.Ferme();
				MessageBox.Show("Le fichier parents.csv ne contient pas tout les champs ou la deuxième ligne ne les présente pas.");
				return;
			}
*/
			//Etape 2 Création des fichiers de sortie
			pronote.EcrireLigne(
"Nom"+";"+
"Prenom"+";"+
"Date de naissance"+";"+
"Identifiant pronote"+";"+
"Mot de passe"+";"+
"Classe"+";"+
"Nom resp. légal1"+";"+
"Prénom resp. légal1"+";"+
"Identifiant"+";"+
"Mot de passe"+";"+
/*"Ligne 1 Adresse légal1"+";"+
"Ligne 2 Adresse légal1"+";"+
"Ligne 3 Adresse légal1"+";"+
"Ligne 4 Adresse légal1"+";"+
"Civilité resp. légal1"+";"+
"Commune resp. légal1"+";"+
"Code postal resp. légal1"+";"+
"Tel maison resp. légal1"+";"+
"Tel travail resp. légal1"+";"+
"Tel mobile resp. légal1"+";"+
"Courriel resp. légal1"+";"+
"Com. adresse légal1"+";"+
"Autorise SMS légal1"+";"+*/
"Nom resp. légal2"+";"+
"Prénom resp. légal2"+";"+
"Identifiant"+";"+
"Mot de passe"+//","+
/*"Ligne 1 Adresse légal2"+";"+
"Ligne 2 Adresse légal2"+";"+
"Ligne 3 Adresse légal2"+";"+
"Ligne 4 Adresse légal2"+";"+
"Civilité resp. légal2"+";"+
"Commune resp. légal2"+";"+
"Code postal resp. légal2"+";"+
"Tel maison resp. légal2"+";"+
"Tel travail resp. légal2"+";"+
"Tel mobile resp. légal2"+";"+
"Courriel resp. légal2"+";"+
"Com. adresse légal2"+";"+
"Autorise SMS légal2*/"\n");

			//Recherche des classes
			ArrayList classes=new ArrayList();
			elv = new StreamReader("ent.csv");
			elv.ReadLine();
			elv.ReadLine();
			while (!elv.EndOfStream)//Pour chaque classe
			{
				line = elv.ReadLine();
				FonctionCLR.element(line,classe, out sclasse, ',');
				classes.Sort();
				if (classes.BinarySearch(sclasse)<0)
				{
//					comp2=classes.BinarySearch(sclasse);
//					comp2=classes.BinarySearch("\"TSMU1\"");
//					MessageBox.Show(classes.BinarySearch("TSMU1").ToString());
					classes.Add(sclasse);
				}
			}
//			MessageBox.Show(classes.Count.ToString());

			//Préparation du fichier AdeleEnt.csv
			adele_ent.WriteLine("Extraction des comptes utilisateurs\n" +
"Etablissement,UID,Civilité,Nom,Prénom,Catégorie,Identifiant,Mot de passe,Classe");

/*			while (!elv.EndOfStream)
			{
				line = elv.ReadLine();
				line2=null;
				FonctionCLR.element(line,categorie, out scategorie, ',');

				if(scategorie=="Elève")
				{
					comp=0;
					while (FonctionCLR.element(line,comp, out str, ',')!=-1)
					{
						if(comp==id)
						{
							//Si le nom fait plus de 20 char
							//Pour faire simple, on considère qu'un doublon de plus de 20 caractères est impossible
							//Si ca arrive, on traitera à la main.
							if (str.Length>20)//Si le nom fait plus de 20 char
							{
								str=str.Substring(0,20);
							}
						}
						line2+=str;
						if (comp!=id)
						{
							line2+=',';
						}
						comp++;
					}
					adele_ent.WriteLine(line2);
				}
			}
			elv.Close();
			adele_ent.Close();*/

			//Début du traitement
			foreach (string cclasse in classes)//Pour chaque classe
			{
				elv = new StreamReader("ent.csv");
				elv.ReadLine();
				elv.ReadLine();
				elv.ReadLine();
				while (!elv.EndOfStream & cclasse!="")//Pour chaque eleve dans l'ENT
				{
					line = elv.ReadLine();
					FonctionCLR.element(line,categorie, out scategorie, ',');

					if(scategorie=="Elève")
					{
						FonctionCLR.element(line,classe, out sclasse, ',');
						if (sclasse == cclasse)
						{
							FonctionCLR.element(line,etablissement, out setablissement, ',');
							FonctionCLR.element(line,uid, out suid, ',');
							FonctionCLR.element(line,civilite, out scivilite, ',');
							FonctionCLR.element(line,nom, out snom, ',');
							FonctionCLR.element(line,prenom, out sprenom, ',');
							FonctionCLR.element(line,id, out slogin, ',');
							FonctionCLR.element(line,mdp, out smdp, ',');
							FonctionCLR.element(line,respNom1, out srespNom1, ',');
							FonctionCLR.element(line,respPrenom1, out srespPrenom1, ',');
							FonctionCLR.element(line,respID1, out srespID1, ',');
							FonctionCLR.element(line,respNom2, out srespNom2, ',');
							FonctionCLR.element(line,respPrenom2, out srespPrenom2, ',');
							FonctionCLR.element(line,respID2, out srespID2, ',');

							srespMDP1="";
							srespMDP2="";

							//Recherche de la date de naissance
							//Récup nom siecle car c'est celui de pronote.
							//Ce nom est utile pour les noms avec tiret ou apostrophe
							comp=0;
							sbirth="";
							snomrespsco1="";
							sprenomrespsco1="";
							snomrespsco2="";
							sprenomrespsco2="";
							siecle.Seek0();
							line=siecle.LireLigne();

							while (!siecle.EndOfStream)
							{
								line = siecle.LireLigne();
								FonctionCLR.element(line,nomsco, out snomsco, ';');
								FonctionCLR.element(line,prenomsco, out sprenomsco, ';');
								FonctionCLR.element(line,classesco, out str, ';');

								if (snomsco.Replace('\'','.')==snom & sprenomsco.Replace('\'','.')==sprenom & cclasse==str)
								{
									FonctionCLR.element(line,nomrespsco1, out snomrespsco1, ';');
									FonctionCLR.element(line,prenomrespsco1, out sprenomrespsco1, ';');
									FonctionCLR.element(line,nomrespsco2, out snomrespsco2, ';');
									FonctionCLR.element(line,prenomrespsco2, out sprenomrespsco2, ';');
									if(srespID1=="")
									{
										comp=1;
										FonctionCLR.element(line,birth, out sbirth, ';');
										break;
									}
									else
									{
										if((snomrespsco1.Replace('\'','.')==srespNom1 &
										    sprenomrespsco1.Replace('\'','.')==srespPrenom1)|
											(snomrespsco1.Replace('\'','.')==srespNom2 &
											sprenomrespsco1.Replace('\'','.')==srespPrenom2))
										{
											comp=1;
											FonctionCLR.element(line,birth, out sbirth, ';');
											break;
										}
									}
								}
							}

							//Recherche du MDP des resp. légaux
							comp=0;
							comp3=0;
							if(srespID1!="" & srespID2!="")
							{
								comp3=2;
							}
							else if (srespID1!="" ^ srespID2!="")
							{
								comp3=1;
							}
							respent = new StreamReader("ent.csv");
							line = respent.ReadLine();
							line = respent.ReadLine();
							saverespID1=srespID1;
							saverespID2=srespID2;
							while (!respent.EndOfStream & comp!=comp3)
							{
								line = respent.ReadLine();
								FonctionCLR.element(line,categorie, out str, ',');
								if(str=="Parent")
								{
									FonctionCLR.element(line,id, out str, ',');

									if (str==saverespID1)
									{
										FonctionCLR.element(line,nom, out str, ',');
										FonctionCLR.element(line,prenom, out str2, ',');
										if(snomrespsco1.Replace('\'','.')==str &
										    sprenomrespsco1.Replace('\'','.')==str2)
										{
											FonctionCLR.element(line,id, out srespID1, ',');
											FonctionCLR.element(line,mdp, out srespMDP1, ',');
										}
										else if(snomrespsco2.Replace('\'','.')==str &
										    sprenomrespsco2.Replace('\'','.')==str2)
										{
											FonctionCLR.element(line,id, out srespID2, ',');
											FonctionCLR.element(line,mdp, out srespMDP2, ',');
										}
										else
										{
											MessageBox.Show("Echec dans le traitement du parent suivant:\n Nom siecle: "+str+" "+str2
											               +"\nNom siecle parent1 "+snomrespsco1+" "+sprenomrespsco1
											              +"\nNom siecle parent2 "+snomrespsco2+" "+sprenomrespsco2
											              +"\nNom de l'élève "+snom+" "+sprenom
											              +"\nCela peut arriver si l'élève à changer de classe ou"
											              +"\nsi les noms prénoms sont légèrement différent."
											             +"\nCe parent et son enfants ne seront pas mis à jour dans Pronote pour ne pas recréer leur compte par erreur.\n");
										}
										comp++;
									}

									if (str==saverespID2)
									{
										FonctionCLR.element(line,nom, out str, ',');
										FonctionCLR.element(line,prenom, out str2, ',');
										if(snomrespsco1.Replace('\'','.')==str &
										    sprenomrespsco1.Replace('\'','.')==str2)
										{
											FonctionCLR.element(line,id, out srespID1, ',');
											FonctionCLR.element(line,mdp, out srespMDP1, ',');
										}
										else if(snomrespsco2.Replace('\'','.')==str &
										    sprenomrespsco2.Replace('\'','.')==str2)
										{
											FonctionCLR.element(line,id, out srespID2, ',');
											FonctionCLR.element(line,mdp, out srespMDP2, ',');
										}
										else
										{
											MessageBox.Show("Echec dans le traitement du parent suivant:\n Nom siecle: "+str+" "+str2
											               +"\nNom siecle parent1 "+snomrespsco1+" "+sprenomrespsco1
											              +"\nNom siecle parent2 "+snomrespsco2+" "+sprenomrespsco2
											              +"\nNom de l'élève "+snom+" "+sprenom
											              +"\nCela peut arriver si l'élève à changer de classe ou"
											              +"\nsi les noms prénoms sont légèrement différent."
											             +"\nCe parent et son enfants ne seront pas mis à jour dans Pronote pour ne pas recréer leur compte par erreur.\n");
										}
										comp++;
									}
								}
							}
							
							//recherche inversé pour le cas des élèves sans parents dans l'ent
							if(comp3==0)
							{
								while (!respent.EndOfStream & comp<2)
								{
									line = respent.ReadLine();
									FonctionCLR.element(line,categorie, out str, ',');
									if(str=="Parent")
									{
										//On gere en plus les noms
										//avec tiret ou apostrophe
										//et dans l'ordre siecle
										//pour eviter la permutation resp1 et 2
										FonctionCLR.element(line,nom, out str, ',');
										FonctionCLR.element(line,prenom, out str2, ',');
										if(snomrespsco1.Replace('\'','.')==str &
										    sprenomrespsco1.Replace('\'','.')==str2)
//											str==snomrespsco1 & str2==sprenomrespsco1)
										{//1er responsable de siecle trouvé
											FonctionCLR.element(line,nom,out srespNom1,',');
											FonctionCLR.element(line,prenom,out srespPrenom1,',');
											FonctionCLR.element(line,id, out srespID1, ',');
											FonctionCLR.element(line,mdp, out srespMDP1, ',');
											comp++;
										}
										else if(snomrespsco2.Replace('\'','.')==str &
										    sprenomrespsco2.Replace('\'','.')==str2)
										{//2nd responsable de siecle trouvé
											FonctionCLR.element(line,nom,out srespNom2,',');
											FonctionCLR.element(line,prenom,out srespPrenom2,',');
											FonctionCLR.element(line,id, out srespID2, ',');
											FonctionCLR.element(line,mdp, out srespMDP2, ',');
											comp++;
										}
/*										FonctionCLR.element(line,respID1, out str, ',');
										if (str==slogin)//c'est un responsable
										{
											comp++;
										}
										FonctionCLR.element(line,respID2, out str, ',');
										if (str==slogin)//c'est un responsable
										{
											comp++;
										}
										FonctionCLR.element(line,eleveID3, out str, ',');
										if (str==slogin)//c'est un responsable
										{
											comp++;
										}

										if (comp==1)
										{
											FonctionCLR.element(line,nom,out srespNom1,',');
											FonctionCLR.element(line,prenom,out srespPrenom1,',');
											FonctionCLR.element(line,id, out srespID1, ',');
											FonctionCLR.element(line,mdp, out srespMDP1, ',');
										}
										else if(comp==2)
										{
											FonctionCLR.element(line,nom,out srespNom2,',');
											FonctionCLR.element(line,prenom,out srespPrenom2,',');
											FonctionCLR.element(line,id, out srespID2, ',');
											FonctionCLR.element(line,mdp, out srespMDP2, ',');
											break;
										}*/
									}
								}
							}
							respent.Close();

							//Si le nom fait plus de 20 char
							//Pour faire simple, on considère qu'un doublon de plus de 20 caractères est impossible
							//Si ca arrive, on traitera à la main.
							if (slogin.Length>20)//Si le nom fait plus de 20 char
							{
								slogin2=slogin.Substring(0,20);
							}
							else
							{
								slogin2=slogin;
							}

							//Est ce un eleve déjà généré ?
							comp=0;
							deltar = new StreamReader("Delta Elv.txt");//Delta
							while (!deltar.EndOfStream)
							{
								line = deltar.ReadLine();
//									FonctionCLR.element(line,0, out delta_id, '\n');
								if (line.ToLower()==snom.ToLower()+" "+sprenom.ToLower()+" "+sclasse)
								{
									comp=1;
									break;
								}
							}
							deltar.Close();

							if(comp==0)
							{
								richTextBox1.Text+=snom+" "+sprenom+" "+slogin+"\n";
							}
							if(srespID1=="" & srespID2=="")
							{
								richTextBox1.Text+="L'élève "+snom+" "+sprenom+" "+slogin+" n'a pas de responsable légal\n";
							}
							if(sbirth=="")
							{
								richTextBox1.Text+="L'élève "+snom+" "+sprenom+" "+slogin+" n'a pas de date de naissance ou classe différente\n";
							}

							if(comp==0)//Eleve nouveau
							{
								//Fichier pour etiquette
								lelv.EcrireLigne(DateTime.Today.ToString("dd/MM/yyyy")+"\n");
								if (slogin.Length>20)//Si le nom fait plus de 20 char
								{
									lelv.EcrireLigne("Identifiant ENT et Pronote,"+slogin+",Mot de passe,"+smdp+"\n");
									lelv.EcrireLigne("Identifiant Prévert,"+slogin2+",Mot de passe,"+smdp+","+sclasse+"\n");
									if(sbirth=="")
									{
										lelv.EcrireLigne("Pronote actif dans 1 semaine environ\n");
									}
									else
									{
										lelv.EcrireLigne("\n");
									}
								}
								else
								{
									lelv.EcrireLigne("Identifiant Unique,"+slogin+",Mot de passe,"+smdp+","+sclasse+"\n");
									if(sbirth=="")
									{
										lelv.EcrireLigne("Pronote actif dans 1 semaine environ\n\n");
									}
									else
									{
										lelv.EcrireLigne("\n\n");
									}
								}
								comp2++;
							}

							//Fichier script
							//script.EcrireLigne("NET USER "+slogin2+" "+smdp+"\n");
							//string aaa;
							//int ee;
							//ee=slogin.IndexOf('.',0);
							//aaa=slogin.Substring(ee+1)+" "+slogin.Substring(0,ee);

							/*script.EcrireLigne("Set VrTemp=\"zzz\"\n");
							script.EcrireLigne("for /f \"delims=\" %%i in ('dsquery user -samid \""+slogin2+"\"') do Set VrTemp=%%i\n");
							script.EcrireLigne("dsmod user %VrTemp% -tel "+sinesco+"\n");
							script.EcrireLigne("if %VrTemp%==\"zzz\" pause\n");*/

							//fichier pour pronote
							//Si pas de date de naissance pronote créé un nouvel élève donc
							//je n'ajoute pas cet élève.
							if(sbirth!="")
							{
								pronote.EcrireLigne(snomsco+";"+sprenomsco+";"+sbirth+";"+slogin+";"+smdp+";"+sclasse+";"
									    +snomrespsco1+";"+sprenomrespsco1+";"+srespID1+";"+srespMDP1+";"/*+sresp13+";"+sresp14+";"
									    +sresp114+";;"+sresp15+";"
										+sresp16+";"+sresp17+";"+sresp18+";"+sresp19+";"+sresp110+";"
										+sresp111+";"+sresp112+";"+sresp113+","*/
			
										+snomrespsco2+";"+sprenomrespsco2+";"+srespID2+";"+srespMDP2/*+";"+sresp23+";"+sresp24+";"
										+sresp214+";;"+sresp25+";"
										+sresp26+";"+sresp27+";"+sresp28+";"+sresp29+";"+sresp210+";"
										+sresp211+";"+sresp212+";"+sresp213*/
								+"\n");
							}

							//Fichier AdeleEnt
/*							if(sbirth=="")
							{
								sbirth="01/01/1990";
							}*/
							//Conversion de date
//							FonctionCLR.element(sbirth,0,out jour,'/');
//							FonctionCLR.element(sbirth,1,out mois,'/');
//							FonctionCLR.element(sbirth,2,out année,'/');
//							sbirth=mois+'/'+jour+'/'+année;
//							MessageBox.Show(sbirth);

								adele_ent.WriteLine(setablissement+","+suid+","+scivilite+","+snom+","+
							                  sprenom+","+scategorie+","+slogin2+","+smdp+","+sclasse);

		/*						MessageBox.Show(snom+"	"+sprenom+"	"+slogin2+"	"+smdp+"	"+sbirth+"	"+sclasse+"	"
							                    +sresp11+"	"+sresp12+"	"+srespentIDc+"	"+srespentMDP+"	"+sresp13+"	"+sresp14+"	"+sresp15+"	"
								+sresp16+"	"+sresp17+"	"+sresp18+"	"+sresp19+"	"+sresp110+"	"
								+sresp111+"	"+sresp112+"	"+sresp113+"	"
		
								+sresp21+"	"+sresp22+"	"+srespentID2c+"	"+srespentMDP2+"	"+sresp23+"	"+sresp24+"	"+sresp25+"	"
								+sresp26+"	"+sresp27+"	"+sresp28+"	"+sresp29+"	"+sresp210+"	"
								+sresp211+"	"+sresp212+"	"+sresp213
								+"\n");
							}*/
							/*else//pas trouvé ou classe différente
							{
								if(sinesco.Length==0)//Pas d'INE ou de classe
								{
									richTextBox1.Text+="L'élève "+snom+" "+sprenom+" n'a pas d'INE ou de classe\n";
								}
								else//Pas trouvé
								{
									richTextBox1.Text+="L'élève "+snom+" "+sprenom+" n'a pas été trouvé ou est en cours de changement de classe\n";
								}
							}*/
						}
					}
				}
			}
			deltaw = new StreamWriter("Delta Elv.txt",true);//Delta
			deltaw.WriteLine('/'+DateTime.Today.ToString("dd/MM/yyyy")+"\n");
			deltaw.Write(richTextBox1.Text+'\n');
			deltaw.Close();

			label1.Text=comp2.ToString()+" entrées traités";
			//script.Ferme();
			siecle.Ferme();
			adele_ent.Close();
			pronote.Ferme();
			lelv.Ferme();
			respent.Close();
			elv.Close();
		}

		void Button2Click(object sender, EventArgs e)//profs
		{
			string line, str, slogin, slogin2="", smdp, snom, sprenom, delta_id;
			byte comp=0, nom=0, prenom=0, id=0, mdp=0;
			int comp2 =0, ret=0, elvid=0;

			StreamReader profs;
			try{
				profs = new StreamReader("profs.csv");//Fichier ENT
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message+"\nCe fichier est une extraction de l'ENT avec les champs Nom,Prénom,identifiant et mot de passe (ordre indiférent)");
				return;
			}

			if (!Directory.Exists("prof"))
			    {
			    	Directory.CreateDirectory("prof");
			    }
			if (!File.Exists("Delta Prof.txt"))
			    {
					FileStream cr = File.Create("Delta Prof.txt");
					cr.Close();
			    }

			line=profs.ReadLine();
			line=profs.ReadLine();

			//Etape 1
			richTextBox1.Text="";
			label1.Text="";
			for(comp=0;-1!=FonctionCLR.element(line,comp, out str, ',');comp++)
			{
//Nom,Prénom,Identifiant,Mot de passe
				switch (str)
				{
				case "Nom":
					nom=comp;
					ret=ret+1;
					break;
				case "Prénom":
				case "Pr�nom":
				case "Prenom":
					prenom=comp;
					ret=ret+1;
					break;
				case "Identifiant":
					id=comp;
					ret=ret+1;
					break;
				case "Mot de passe":
					mdp=comp;
					ret=ret+1;
					break;
				}
			}
			profs.Close();
			if(ret<4)
			{
				profs.Close();
				MessageBox.Show("Le fichier profs.csv ne contient pas tout les champs.(Nom, Prénom, Identifiant, Mot de passe)");
				return;
			}
			StreamReader deltar;
			StreamWriter deltaw;
			File.Delete("prof/Liste Compte Profs.csv");
			File.Delete("prof/Compte Grr.csv");
			File.Delete("prof/Pronote Profs.txt");
			File.Delete("prof/MDP Profs.bat");
			File.Delete("prof/MDP Photocop.bat");
			FonctionFichierBinaireCLR lprof = new FonctionFichierBinaireCLR("prof/Liste Compte Profs.csv","w");//liste code
			FonctionFichierBinaireCLR grr = new FonctionFichierBinaireCLR("prof/Compte Grr.csv","w");//grr
			FonctionFichierBinaireCLR pronote = new FonctionFichierBinaireCLR("prof/Pronote Profs.csv","w");//Pronote
			FonctionFichierBinaireCLR mdpprof = new FonctionFichierBinaireCLR("prof/MDP Profs.bat","w");//Changement MDP
			FonctionFichierBinaireCLR mdpphotocop = new FonctionFichierBinaireCLR("prof/MDP Photocop.bat","w");//Changement MDP

			StreamWriter eleve = new StreamWriter("prof/eleve.tmp");
			StreamWriter option = new StreamWriter("prof/option.tmp");
			StreamWriter structure = new StreamWriter("prof/structure.tmp");

			//Etape 2 Création des fichiers de sortie
			eleve.Write("<?xml version=\"1.0\" encoding=\"ISO-8859-15\"?>\n"+
			    "<BEE_ELEVES VERSION=\"1.8\">\n"+
					"  <PARAMETRES>\n"+
					"    <UAJ>0000</UAJ>\n"+
					"    <ANNEE_SCOLAIRE>2012</ANNEE_SCOLAIRE>\n"+
					"    <DATE_EXPORT>23/10/2012</DATE_EXPORT>\n"+
					"    <HORODATAGE>23/10/2012 13:21:42</HORODATAGE>\n"+
					"  </PARAMETRES>\n"+
					"  <DONNEES>\n"+
					"    <ELEVES>\n");
			option.WriteLine("    <OPTIONS>");
			structure.WriteLine("    <STRUCTURES>");

			profs = new StreamReader("profs.csv");
			line=profs.ReadLine();
			line=profs.ReadLine();
			while (!profs.EndOfStream)
			{
				line = profs.ReadLine();
				FonctionCLR.element(line,id, out slogin, ',');
				FonctionCLR.element(line,nom, out snom, ',');
				FonctionCLR.element(line,prenom, out sprenom, ',');
				FonctionCLR.element(line,mdp, out smdp, ',');

				//Est ce un prof déjà généré ?
				comp=0;
				deltar = new StreamReader("Delta Prof.txt");//Delta
				while (!deltar.EndOfStream & comp!=1)
				{
					line = deltar.ReadLine();
					FonctionCLR.element(line,0, out delta_id, '\n');
					if (delta_id==slogin)
					{
						comp=1;
					}
				}
				deltar.Close();

				if (slogin.Length>20)//Si le nom fait plus de 20 char
				{
					slogin2=slogin.Substring(0,20);
				}
				else
				{
					slogin2=slogin;
				}
				if(comp==0)//nouveau prof
				{
					lprof.EcrireLigne(DateTime.Today.ToString("dd/MM/yyyy")+"\n");
					//Fichier pour mot de passe
					mdpprof.EcrireLigne("NET USER "+slogin2+" "+smdp+"\n");
					//Liste compte Grr
					grr.EcrireLigne(slogin2+";"+snom+";"+sprenom+";"+smdp+";-;utilisateur;actif;local"+"\n");
					pronote.EcrireLigne(snom+";"+sprenom+";"+slogin2+";"+smdp+"\n");

					if (slogin.Length>20)//Si le nom fait plus de 20 char
					{
						lprof.EcrireLigne("Identifiant ENT,"+slogin+",Mot de passe,"+smdp+"\n");

						lprof.EcrireLigne("Identifiant Prévert Pronote,"+slogin2+",Mot de passe,"+smdp+"\n");
						richTextBox1.Text+=slogin+"\n";
						deltaw = new StreamWriter("Delta Prof.txt",true);//Delta
						deltaw.WriteLine(slogin);
						deltaw.Close();
						slogin=slogin2;
					}
					else
					{
						lprof.EcrireLigne("Identifiant Unique,"+slogin+",Mot de passe,"+smdp+"\n\n");
						richTextBox1.Text+=slogin+"\n";
						deltaw = new StreamWriter("Delta Prof.txt",true);//Delta
						deltaw.WriteLine(slogin);
						deltaw.Close();
					}
					comp2++;
				}
				snom=snom.Substring(0,3);
				sprenom=sprenom.Substring(0,3);
				slogin2=snom+"."+sprenom;
				smdp=snom.Substring(0,1)+sprenom.Substring(0,1)+"1234";
				elvid+=1;
				eleve.Write(
"      <ELEVE ELEVE_ID=\""+elvid.ToString()+"\" ELENOET=\""+elvid.ToString()+"\">\n"+
"        <ID_NATIONAL>0000000000G</ID_NATIONAL>\n"+
"        <ELENOET>"+elvid.ToString()+"</ELENOET>\n"+
"        <ID_ELEVE_ETAB>0000</ID_ELEVE_ETAB>\n"+
"        <NOM>"+snom+"</NOM>\n"+
"        <PRENOM>"+sprenom+"</PRENOM>\n"+
"        <DATE_NAISS>01/01/1980</DATE_NAISS>\n"+
"        <DOUBLEMENT>0</DOUBLEMENT>\n"+
"        <TEL_PROFESSIONNEL></TEL_PROFESSIONNEL>\n"+
"        <CODE_PAYS>100</CODE_PAYS>\n"+
"        <ACCEPTE_SMS>0</ACCEPTE_SMS>\n"+
"        <DATE_MODIFICATION>01/01/1980</DATE_MODIFICATION>\n"+
"        <DATE_SORTIE>01/01/1980</DATE_SORTIE>\n"+
"        <MEL></MEL>\n"+
"        <CODE_REGIME>0</CODE_REGIME>\n"+
"        <DATE_ENTREE>01/09/2009</DATE_ENTREE>\n"+
"        <VILLE_NAISS></VILLE_NAISS>\n"+
"        <CODE_MOTIF_SORTIE>19</CODE_MOTIF_SORTIE>\n"+
"        <TEL_PERSONNEL>00 00 00 00 00</TEL_PERSONNEL>\n"+
"        <CODE_SEXE>1</CODE_SEXE>\n"+
"        <TEL_PORTABLE></TEL_PORTABLE>\n"+
"        <CODE_PAYS_NAT>100</CODE_PAYS_NAT>\n"+
"        <CODE_DEPARTEMENT_NAISS>030</CODE_DEPARTEMENT_NAISS>\n"+
"        <CODE_COMMUNE_INSEE_NAISS>30000</CODE_COMMUNE_INSEE_NAISS>\n"+
"        <ADHESION_TRANSPORT>0</ADHESION_TRANSPORT>\n"+
"        <CODE_PROVENANCE>1</CODE_PROVENANCE>\n"+
"        <SCOLARITE_AN_DERNIER>\n"+
"          <CODE_MEF>21231013110</CODE_MEF>\n"+
"          <CODE_STRUCTURE>PHOTOCOP</CODE_STRUCTURE>\n"+
"          <CODE_RNE>00000</CODE_RNE>\n"+
"          <CODE_NATURE>306</CODE_NATURE>\n"+
"          <SIGLE>LPO</SIGLE>\n"+
"          <DENOM_PRINC>LYCEE POLYVALENT</DENOM_PRINC>\n"+
"          <DENOM_COMPL>XXXXX</DENOM_COMPL>\n"+
"          <LIGNE1_ADRESSE></LIGNE1_ADRESSE>\n"+
"          <BOITE_POSTALE></BOITE_POSTALE>\n"+
"          <MEL></MEL>\n"+
"          <TELEPHONE></TELEPHONE>\n"+
"          <CODE_COMMUNE_INSEE>30243</CODE_COMMUNE_INSEE>\n"+
"          <LL_COMMUNE_INSEE>SAINT-CHRISTOL-LES-ALES</LL_COMMUNE_INSEE>\n"+
"        </SCOLARITE_AN_DERNIER>\n"+
"      </ELEVE>\n"
);
				option.Write(
"      <OPTION ELEVE_ID=\""+elvid.ToString()+"\" ELENOET=\""+elvid.ToString()+"\">\n"+
"        <OPTIONS_ELEVE>\n"+
"          <NUM_OPTION>1</NUM_OPTION>\n"+
"          <CODE_MODALITE_ELECT>O</CODE_MODALITE_ELECT>\n"+
"          <CODE_MATIERE>030201</CODE_MATIERE>\n"+
"        </OPTIONS_ELEVE>\n"+
"      </OPTION>\n"
);
				structure.Write(
"      <STRUCTURES_ELEVE ELEVE_ID=\""+elvid.ToString()+"\" ELENOET=\""+elvid.ToString()+"\">\n"+
"        <STRUCTURE>\n"+
"          <CODE_STRUCTURE>PHOTOCOP</CODE_STRUCTURE>\n"+
"          <TYPE_STRUCTURE>D</TYPE_STRUCTURE>\n"+
"        </STRUCTURE>\n"+
"      </STRUCTURES_ELEVE>\n"
);
				mdpphotocop.EcrireLigne("NET USER "+slogin2+" "+smdp+"\n");
			}
			label1.Text=comp2.ToString()+" entrées traités";

			//réunion des fichiers
			eleve.Write("    </ELEVES>\n");
			eleve.Close();
			option.Write("    </OPTIONS>\n");
			option.Close();
			structure.Write("    </STRUCTURES>\n"+
						"  </DONNEES>\n"+
						"</BEE_ELEVES>\n");
			structure.Close();
			Process concat = new Process();
			concat.StartInfo.FileName="cmd.exe";
			concat.StartInfo.Arguments="/c type prof\\eleve.tmp prof\\option.tmp prof\\structure.tmp > prof\\Photocop.xml";

			concat.Start();

			profs.Close();

			grr.Ferme();
			pronote.Ferme();
			lprof.Ferme();
			mdpprof.Ferme();
			mdpphotocop.Ferme();

			//File.Delete("prof/eleve.tmp");
			//File.Delete("prof/option.tmp");
			//File.Delete("prof/structure.tmp");
		}
		void AProposToolStripMenuItemClick(object sender, EventArgs e)
		{
			MessageBox.Show("Rentree Version 2.0\nGénérateur de fichiers pour GRR, Pronote, Adèle2K3, Photocopieur et Liste de compte\nPour le lycée XXX\nC.L.R.2012");
		}

		void RemiseÀZéroDesDeltasToolStripMenuItemClick(object sender, EventArgs e)
		{
			File.Delete("Delta Elv.txt");
			File.Delete("Delta Prof.txt");
		}

		void FermerToolStripMenuItemClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
	static public class FonctionCLR
	{
		static public string Remplace(string str)
		{
			return str.Replace(" ","-");
		}
		static public int element(string line, byte nb, out string retour, char sep)
		{
			int comp=0, oldcomp=0, comp2=0;
			char[] chr = new char[512];

			if (line==null)
			{
				retour="";
				return 0;
			}
			while (comp!=-1)
			{
				comp = line.IndexOf(sep,oldcomp);

				if (comp==-1)
				{
					if(comp2<nb)//le champ n'existe pas car il y en a moins que demandé
					{
						retour="";
						return -1;
					}
					else//seul champ dispo ou dernier
					{
						comp=line.Length;
					}
				}
				if (comp2==nb)
				{
					line.CopyTo(oldcomp,chr,0,comp-oldcomp);
					retour = new string(chr,0,comp-oldcomp);//La seule solution pour convertir char* en string
					return comp2;
				}
				comp2++;
				if (comp!=line.Length)
				{
					oldcomp=comp+1;
				}
				else
				{
					oldcomp=comp;
				}
			}
			retour="";
			return comp;
		}
	}
	public class FonctionFichierBinaireCLR
	{
		public bool EndOfStream =false;
		string mode;
		FileStream fs;
		BinaryReader fr;
		BinaryWriter fw;

		public FonctionFichierBinaireCLR(string str, string str2)
		{
			mode=str2.ToLower();
			fs = new FileStream(str,FileMode.OpenOrCreate);
			if(mode.IndexOf("r")==0)
			{
				fr = new BinaryReader(fs,Encoding.ASCII);
			}
			if(mode.IndexOf("w")==0)
			{
				fw = new BinaryWriter(fs,Encoding.ASCII);
			}
		}
		public void Ferme()
		{
			if(mode.IndexOf("r")==0)
			{
				fr.Close();
			}
			if(mode.IndexOf("w")==0)
			{
				fw.Close();
			}
			//fs.Close();
		}
		public void EcrireLigne(string str)
		{
			if(mode.IndexOf("w")==0)
			{
				//BinaryWriter fw = new BinaryWriter(fs);
	
				foreach (char chr in str)
				{
					if(chr==10)
					{
						fw.Write((byte)13);
					}
					fw.Write((byte)chr);
				}
			}
		}
		public void Seek0()
		{
			fs.Seek(0,0);
			EndOfStream=false;
		}
		public string LireLigne()
		{
			byte by=0;
			bool ligne=false;
			char[] chr = new char[4096];
			string str = "";
			int comp = 0;

			if(mode.IndexOf("r")==0)
			{
				for(comp=0;EndOfStream!=true&ligne!=true;comp++)
				{
					try
					{
						by = fr.ReadByte();
					}
					catch(EndOfStreamException)
					{
						EndOfStream=true;
					}
					if(by==0xd | EndOfStream==true | by==61 | by==34)
					{
						comp--;
					}
					else if(by==0xa)
					{
						ligne=true;
					}
					else
					{
						chr[comp] = (char)by;
					}
				}
				if(comp!=0)
				{
					str = new string(chr,0,comp-1);
				}
				return str;
			}
			return "";
		}
	}
}
