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
//using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using System.IO;
using System.Text;

namespace RentreeGPAL
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
			string line,line2,str, slogin, slogin2,
				smdp, sclasse, sprenom, snom,
				snomsco="", sprenomsco="", sclassesco="", sbirth="",sinesco="",
				srespentnom="",srespentprenom="",srespentID="",srespentMDP="",
				srespentnom2="",srespentprenom2="",srespentID2="",srespentMDP2="",

				sresp11="", sresp12="",sresp13="", sresp14="",/*sresp15="",
				sresp16="", sresp17="",sresp18="", sresp19="",sresp110="",
				sresp111="", sresp112="",sresp113="",sresp114="",*/

				sresp21="", sresp22="",sresp23="", sresp24=""/*,sresp25="",
				sresp26="", sresp27="",sresp28="", sresp29="",sresp210="",
				sresp211="", sresp212="",sresp213="",sresp214=""*/;

			byte comp=0, comp3=0, nom=0, prenom=0, id=0,
				mdp=0, classe=0, birth=0,
				nomsco=0, prenomsco=0,classesco=0,inesco=0,
				respentnom=0,respentprenom=0,respentID=0,respentMDP=0,

				resp11=0, resp12=0,resp13=0, resp14=0,/*resp15=0,
				resp16=0, resp17=0,resp18=0, resp19=0,resp110=0,
				resp111=0, resp112=0,resp113=0,resp114=0,*/

				resp21=0, resp22=0,resp23=0, resp24=0/*,resp25=0,
				resp26=0, resp27=0,resp28=0, resp29=0,resp210=0,
				resp211=0, resp212=0,resp213=0,resp214=0*/;

			byte[] an_array= new byte[64];
			int comp2=0, ret=0;

			FonctionCLR myCLR = new FonctionCLR();

			if (!File.Exists("Delta Elv.txt"))
			    {
					FileStream cr = File.Create("Delta Elv.txt");
					cr.Close();
			    }
			StreamReader deltar;
			StreamWriter deltaw;

			StreamReader elv;
			try{
				elv = new StreamReader("eleves.csv");//Fichier ENT
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message+"\nCe fichier est une extraction de l'ENT avec les champs Nom,Prénom,identifiant et mot de passe (ordre indiférent)");
				return;
			}
			StreamReader respent;
			try{
				respent = new StreamReader("parents.csv");//Fichier responsable ENT
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message+"\nCe fichier est une extraction de l'ENT avec les champs Nom,Prénom,identifiant et mot de passe (ordre indiférent)");
				return;
			}
			if (!File.Exists("sconet.csv"))
			    {
					MessageBox.Show("Fichier sconet.csv introuvable.\nCe fichier est une extraction Eleves Complete de Sconet avec les champs:\n" +
				                "Nom	Prénom 1	Date de naissance	Code Structure	Nom resp. légal1	Prénom resp. légal1	Nom resp. légal2	Prénom resp. légal2");
					return;
				}
			FonctionFichierBinaireCLR sconet= new FonctionFichierBinaireCLR("sconet.csv");

			if (!Directory.Exists("eleve"))
			    {
			    	Directory.CreateDirectory("eleve");
			    }
			File.Delete("eleve/Liste Compte Eleves.csv");
			File.Delete("eleve/Pronote.txt");
			File.Delete("eleve/AdeleEnt.csv");
			//File.Delete("eleve/Script.cmd");
			//FonctionFichierBinaireCLR script = new FonctionFichierBinaireCLR("eleve/Script.cmd");//Script
			FonctionFichierBinaireCLR lelv = new FonctionFichierBinaireCLR("eleve/Liste Compte Eleves.csv");//Pour Adele
			FonctionFichierBinaireCLR pronote = new FonctionFichierBinaireCLR("eleve/Pronote.txt");//Pronote
			StreamWriter adele_ent = new StreamWriter("eleve/AdeleEnt.csv");//Liste de codes pour synchro ENT/Adele

			elv.ReadLine();
			line=elv.ReadLine();
			//test du séparateur
			if (line.IndexOf(",")==-1)
			{
				respent.Close();
	
				//script.Ferme();
				sconet.Ferme();
				adele_ent.Close();
				pronote.Ferme();
				lelv.Ferme();
				MessageBox.Show("Le fichier eleve.csv doit avoir , pour séparateur de colone.\n");
				return;
			}
			richTextBox1.Text="";
			label1.Text="";
			//Etape 1
			for(comp=0;-1!=myCLR.element(line,comp, out str, ',');comp++)
			{
//Nom,Prénom,Identifiant,Mot de passe,Classe
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
				case "Classe":
					classe=comp;
					ret=ret+1;
					break;
				}
			}
			elv.Close();
			if(ret<5)
			{
				//script.Ferme();
				sconet.Ferme();
				adele_ent.Close();
				pronote.Ferme();
				lelv.Ferme();
				MessageBox.Show("Le fichier eleves.csv ne contient pas tout les champs ou la première ligne ne les présente pas.");
				return;
			}

			ret=0;
			line=sconet.LireLigne();
			//test du séparateur
			if (line.IndexOf(",")==-1)
			{
				respent.Close();
	
				//script.Ferme();
				sconet.Ferme();
				adele_ent.Close();
				pronote.Ferme();
				lelv.Ferme();
				MessageBox.Show("Le fichier sconet.csv doit avoir , pour séparateur de colone.\n");
				return;
			}
			for(comp=0;-1!=myCLR.element(line,comp, out str, ',');comp++)
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
					resp11=comp;
					ret=ret+1;
					break;
				case "Prénom Resp. Lég. 1":
					resp12=comp;
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
					resp21=comp;
					ret=ret+1;
					break;
				case "Prénom Resp. Lég. 2":
					resp22=comp;
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
			if(ret<9)
			{
				elv.Close();

				//script.Ferme();
				sconet.Ferme();
				adele_ent.Close();
				pronote.Ferme();
				lelv.Ferme();
				MessageBox.Show("Le fichier sconet.csv ne contient pas tout les champs ou la deuxième ligne ne les présente pas.\n" +
				                "Faire une extraction Eleve avec adresses");
				return;
			}
			ret=0;
			line=respent.ReadLine();
			line=respent.ReadLine();
			//test du séparateur
			if (line.IndexOf(",")==-1)
			{
				respent.Close();
	
				//script.Ferme();
				sconet.Ferme();
				adele_ent.Close();
				pronote.Ferme();
				lelv.Ferme();
				MessageBox.Show("Le fichier parent.csv doit avoir , pour séparateur de colone.\n");
				return;
			}

			for(comp=0;-1!=myCLR.element(line,comp, out str, ',');comp++)
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
			sconet.Ferme();
			adele_ent.Close();
			pronote.Ferme();
			lelv.Ferme();
				MessageBox.Show("Le fichier parents.csv ne contient pas tout les champs ou la deuxième ligne ne les présente pas.");
				return;
			}

			//Etape 2 Création des fichiers de sortie
			pronote.EcrireLigne(
"Nom"+","+
"Prenom"+","+
"Identifiant pronote"+","+
"Mot de passe"+","+
"Date de naissance"+","+
"Classe"+","+
"Nom resp. légal1"+","+
"Prénom resp. légal1"+","+
"Identifiant"+","+
"Mot de passe"+","+
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
"Nom resp. légal2"+","+
"Prénom resp. légal2"+","+
"Identifiant"+","+
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
			elv = new StreamReader("eleves.csv");
			elv.ReadLine();
			elv.ReadLine();
			while (!elv.EndOfStream)//Pour chaque classe
			{
				line = elv.ReadLine();
				myCLR.element(line,classe, out sclasse, ',');
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

			//Génération du fichier AdeleEnt.csv
			elv = new StreamReader("eleves.csv");
			line = elv.ReadLine();
			adele_ent.WriteLine(line);
			line = elv.ReadLine();
			adele_ent.WriteLine(line);

			while (!elv.EndOfStream)
			{
				line = elv.ReadLine();
				line2=null;
				for(comp=0;comp<9;comp++)
				{
					myCLR.element(line,comp, out str, ',');
					if(comp==6)
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
					if (comp!=8)
					{
						line2+=',';
					}
				}
				adele_ent.WriteLine(line2);
			}
			elv.Close();
			adele_ent.Close();

			foreach (string cclasse in classes)//Pour chaque classe
			{
				elv = new StreamReader("eleves.csv");
				elv.ReadLine();
				elv.ReadLine();
				while (!elv.EndOfStream)//Pour chaque eleve dans l'ENT
				{
					line = elv.ReadLine();
					myCLR.element(line,classe, out sclasse, ',');

					if (sclasse == cclasse)
					{
						myCLR.element(line,nom, out snom, ',');
						myCLR.element(line,prenom, out sprenom, ',');
						myCLR.element(line,id, out slogin, ',');
						myCLR.element(line,mdp, out smdp, ',');

						//Recherche de l'élève dans sconet
						comp=0;
						sresp11="";
						sresp12="";
						sresp13="";
						sresp14="";
/*						sresp15="";
						sresp16="";
						sresp17="";
						sresp18="";
						sresp19="";
						sresp110="";
						sresp111="";
						sresp112="";
						sresp113="";
						sresp114="";*/
						sresp21="";
						sresp22="";
						sresp23="";
						sresp24="";
/*						sresp25="";
						sresp26="";
						sresp27="";
						sresp28="";
						sresp29="";
						sresp210="";
						sresp211="";
						sresp212="";
						sresp213="";
						sresp214="";*/
						sbirth="";
						sinesco="";
						while (!sconet.EndOfStream & comp!=1)
						{
							line = sconet.LireLigne();
							myCLR.element(line,nomsco, out snomsco, ',');
							myCLR.element(line,prenomsco, out sprenomsco, ',');
							myCLR.element(line,classesco, out sclassesco, ',');
							myCLR.element(line,inesco, out sinesco, ',');

							if (snom==snomsco & sprenom==sprenomsco& sclasse==sclassesco)
							{
								comp=1;
								myCLR.element(line,birth, out sbirth, ',');

								myCLR.element(line,resp11, out sresp11, ',');
								sresp11=sresp11.Replace("'",".");
								myCLR.element(line,resp12, out sresp12, ',');
								myCLR.element(line,resp13, out sresp13, ',');
								myCLR.element(line,resp14, out sresp14, ',');
/*								myCLR.element(line,resp15, out sresp15, ';');
								myCLR.element(line,resp16, out sresp16, ';');
								myCLR.element(line,resp17, out sresp17, ';');
								myCLR.element(line,resp18, out sresp18, ';');
								myCLR.element(line,resp19, out sresp19, ';');
								myCLR.element(line,resp110, out sresp110, ';');
								myCLR.element(line,resp111, out sresp111, ';');
								myCLR.element(line,resp112, out sresp112, ';');
								myCLR.element(line,resp113, out sresp113, ';');
								myCLR.element(line,resp114, out sresp114, ';');
*/

								myCLR.element(line,resp21, out sresp21, ',');
								sresp21=sresp21.Replace("'",".");
								myCLR.element(line,resp22, out sresp22, ',');
								myCLR.element(line,resp23, out sresp23, ',');
								myCLR.element(line,resp24, out sresp24, ',');
/*								myCLR.element(line,resp25, out sresp25, ';');
								myCLR.element(line,resp26, out sresp26, ';');
								myCLR.element(line,resp27, out sresp27, ';');
								myCLR.element(line,resp28, out sresp28, ';');
								myCLR.element(line,resp29, out sresp29, ';');
								myCLR.element(line,resp210, out sresp210, ';');
								myCLR.element(line,resp211, out sresp211, ';');
								myCLR.element(line,resp212, out sresp212, ';');
								myCLR.element(line,resp213, out sresp213, ';');
								myCLR.element(line,resp214, out sresp214, ';');
*/							}
						}
						sconet.Seek0();

						//Si l'élève n'est pas dans sconet, il est probablement sorti de l'établissement
						//ou en cours de changement de classe.
						//Si il n'a pas d'INE, il ne sera pas traité.
						if(comp==1&sinesco.Length!=0)
						{
							//Recherche du login et MDP du premier resp
	
							respent = new StreamReader("parents.csv");
							while (!respent.EndOfStream & comp==1)
							{
								line = respent.ReadLine();
								myCLR.element(line,respentnom, out srespentnom, ',');
								myCLR.element(line,respentprenom, out srespentprenom, ',');
	
								if (srespentnom==sresp11 & srespentprenom==sresp12)
								{
									comp=2;
									myCLR.element(line,respentID, out srespentID, ',');
									myCLR.element(line,respentMDP, out srespentMDP, ',');
								}
							}
							respent.Close();
							respent = new StreamReader("parents.csv");
	
							//Recherche du login et MDP du 2e resp
							while (!respent.EndOfStream & comp==2)
							{
								line = respent.ReadLine();
								myCLR.element(line,respentnom, out srespentnom2, ',');
								myCLR.element(line,respentprenom, out srespentprenom2, ',');
	
								if (srespentnom2==sresp21 & srespentprenom2==sresp22)
								{
									comp=3;
									myCLR.element(line,respentID, out srespentID2, ',');
									myCLR.element(line,respentMDP, out srespentMDP2, ',');
								}
							}
							respent.Close();
	
							//Est ce un eleve déjà généré ?
							comp3=0;
							deltar = new StreamReader("Delta Elv.txt");//Delta
							while (!deltar.EndOfStream & comp3!=1)
							{
								line = deltar.ReadLine();
//									myCLR.element(line,0, out delta_id, '\n');
								if (line.ToLower()==snom.ToLower()+" "+sprenom.ToLower()+" "+slogin)
								{
									comp3=1;
								}
							}
							deltar.Close();

							if(comp3==0)
							{
								richTextBox1.Text+=snom+" "+sprenom+" "+slogin+"\n";
							}

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

							if(comp3==0)//Eleve nouveau
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
						if(sbirth=="")
						{
							sbirth="01/01/1990";
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

						//Conversion de date
//						myCLR.element(sbirth,0,out jour,'/');
//						myCLR.element(sbirth,1,out mois,'/');
//						myCLR.element(sbirth,2,out année,'/');
//						sbirth=mois+'/'+jour+'/'+année;
//						MessageBox.Show(sbirth);


						//fichier pour pronote
						pronote.EcrireLigne(snom+","+sprenom+","+slogin+","+smdp+","+sbirth+","+sclasse+","
							    +sresp11+","+sresp12+","+srespentID+","+srespentMDP/*+";"+sresp13+";"+sresp14+";"
							    +sresp114+";;"+sresp15+";"
								+sresp16+";"+sresp17+";"+sresp18+";"+sresp19+";"+sresp110+";"
								+sresp111+";"+sresp112+";"+sresp113*/+","
	
								+sresp21+","+sresp22+","+srespentID2+","+srespentMDP2/*+";"+sresp23+";"+sresp24+";"
								+sresp214+";;"+sresp25+";"
								+sresp26+";"+sresp27+";"+sresp28+";"+sresp29+";"+sresp210+";"
								+sresp211+";"+sresp212+";"+sresp213*/
						+"\n");
	
	/*						MessageBox.Show(snom+"	"+sprenom+"	"+slogin2+"	"+smdp+"	"+sbirth+"	"+sclasse+"	"
						                    +sresp11+"	"+sresp12+"	"+srespentIDc+"	"+srespentMDP+"	"+sresp13+"	"+sresp14+"	"+sresp15+"	"
							+sresp16+"	"+sresp17+"	"+sresp18+"	"+sresp19+"	"+sresp110+"	"
							+sresp111+"	"+sresp112+"	"+sresp113+"	"
	
							+sresp21+"	"+sresp22+"	"+srespentID2c+"	"+srespentMDP2+"	"+sresp23+"	"+sresp24+"	"+sresp25+"	"
							+sresp26+"	"+sresp27+"	"+sresp28+"	"+sresp29+"	"+sresp210+"	"
							+sresp211+"	"+sresp212+"	"+sresp213
							+"\n");
		*/				}
						else//pas trouvé ou classe différente
						{
							if(sinesco.Length==0)//Pas d'INE ou de classe
							{
								richTextBox1.Text+="L'élève "+snom+" "+sprenom+" n'a pas d'INE ou de classe\n";
							}
							else//Pas trouvé
							{
								richTextBox1.Text+="L'élève "+snom+" "+sprenom+" n'a pas été trouvé ou est en cours de changement de classe\n";
							}
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
			sconet.Ferme();
			adele_ent.Close();
			pronote.Ferme();
			lelv.Ferme();
		}

		void Button2Click(object sender, EventArgs e)//profs
		{
			string line, str, slogin, slogin2="", smdp, snom, sprenom, delta_id;
			byte comp=0, nom=0, prenom=0, id=0, mdp=0;
			int comp2 =0, ret=0;
			FonctionCLR myCLR = new FonctionCLR();

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
			StreamReader deltar;
			StreamWriter deltaw;
			File.Delete("prof/Liste Compte Profs.csv");
			File.Delete("prof/Compte Grr.csv");
			File.Delete("prof/Pronote Profs.csv");
			File.Delete("prof/MDP Profs.bat");
			FonctionFichierBinaireCLR lprof = new FonctionFichierBinaireCLR("prof/Liste Compte Profs.csv");//liste code
			FonctionFichierBinaireCLR grr = new FonctionFichierBinaireCLR("prof/Compte Grr.csv");//grr
			FonctionFichierBinaireCLR pronote = new FonctionFichierBinaireCLR("prof/Pronote Profs.csv");//Pronote
			FonctionFichierBinaireCLR mdpprof = new FonctionFichierBinaireCLR("prof/MDP Profs.bat");//Changement MDP

			line=profs.ReadLine();
			line=profs.ReadLine();

			//Etape 1
			richTextBox1.Text="";
			label1.Text="";
			for(comp=0;-1!=myCLR.element(line,comp, out str, ',');comp++)
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

			//Etape 2 Création des fichiers de sortie
			profs = new StreamReader("profs.csv");
			line=profs.ReadLine();
			line=profs.ReadLine();
			while (!profs.EndOfStream)
			{
				line = profs.ReadLine();
				myCLR.element(line,id, out slogin, ',');
				myCLR.element(line,nom, out snom, ',');
				myCLR.element(line,prenom, out sprenom, ',');
				myCLR.element(line,mdp, out smdp, ',');

				//Est ce un prof déjà généré ?
				comp=0;
				deltar = new StreamReader("Delta Prof.txt");//Delta
				while (!deltar.EndOfStream & comp!=1)
				{
					line = deltar.ReadLine();
					myCLR.element(line,0, out delta_id, '\n');
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
				if(comp==0)
				{
					lprof.EcrireLigne(DateTime.Today.ToString("dd/MM/yyyy")+"\n");
					//Fichier pour mot de passe
					mdpprof.EcrireLigne("NET USER "+slogin2+" "+smdp+"\n");
					//Liste compte Grr
					grr.EcrireLigne(slogin2+";"+snom+";"+sprenom+";"+smdp+";-;utilisateur;actif;local"+"\n");
					pronote.EcrireLigne(snom+"	"+sprenom+"	"+slogin2+"	"+smdp+"\n");

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
			}
			label1.Text=comp2.ToString()+" entrées traités";
			profs.Close();

			grr.Ferme();
			pronote.Ferme();
			lprof.Ferme();
		}

		void AProposToolStripMenuItemClick(object sender, EventArgs e)
		{
			MessageBox.Show("Rentree Version 2.0\nGénérateur de fichiers pour GRR, Pronote, Adèle2K3 et Liste de compte\nPour le lycée XXXX\nC.L.R.2012");
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
	public class FonctionCLR
	{
		public string Remplace(string str)
		{
			return str.Replace(" ","-");
		}
		public int element(string line, byte nb, out string retour, char sep)
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
		FileStream fs;
		BinaryReader fr;
		BinaryWriter fw;

		public FonctionFichierBinaireCLR(string str)
		{
			fs = new FileStream(str,FileMode.OpenOrCreate);
			fr = new BinaryReader(fs,Encoding.ASCII);
			fw = new BinaryWriter(fs,Encoding.ASCII);
		}
		public void Ferme()
		{
			fr.Close();
			fw.Close();
			fs.Close();
		}
		public void EcrireLigne(string str)
		{
//			BinaryWriter fw = new BinaryWriter(fs);

			foreach (char chr in str)
			{
				if(chr==10)
				{
					fw.Write((byte)13);
				}
				fw.Write((byte)chr);
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
	}
}
