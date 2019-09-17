/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 05/03/2010
 * Heure: 12:00
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.

V0.1	Première version. Lecture d'un fichier plan.txt pour avoir les infos sur les salles
		les postes et les étages (1er et 2eme). 20 Machines pas salle.
		Controle de l'autorisation d'execution par le numero de serie windows dans le registre
V0.2	Amélioration sur la lecture de plan.txt
		Ajout du RDC
V1.0	Icones des postes créées dynamiquement avec une collection d'objet.
V1.1	plan.txt en format csv.
V1.2	nouveau lecteur csv.
V2.0	Mosaique.

V3.0	Annulé car le temps restant d'utilisation est court.
		(Annulé)Récupération des infos des salles et des postes par Base SQL.
		Récupération par fichier plan.csv disponible aussi
V4.0	(Annulé)Controle de l'autorisation d'execution
		via un serveur sur le net. Autorisation en fonction de l'IP de l'établissement.
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;
using VncSharp;

namespace Vnc_Manager
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		const byte POSTESMAX=20;
		bool vprof = false, etat_labels=false;
		int nb_poste, poste_fc;
		string salle_courante, poste_courant, label_save, mode, mode_save, path;
		string[,] liste_poste;//0=nom 1=X 2=Y 3=online

		//RDArray ne pas supprimer le "new" à cause d'un bug
		RemoteDesktop[] RDArray= new RemoteDesktop[POSTESMAX];
		Label[] LabelArray= new Label[POSTESMAX];
		Label[] LabelArrayMap=new Label[POSTESMAX];
		PictureBox[] PictureBoxArray=new PictureBox[POSTESMAX];
		FonctionFichierBinaireCLR binfile;

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//

			int comp;
			string nposte,nsalle,line,test;

			poste_courant=System.Environment.MachineName.ToUpper();
			test=System.Environment.GetEnvironmentVariable("USERPROFILE")+"\\ntuser.log";
//simulation
//poste_courant="B01-01";

//			this.SuspendLayout();
			//Fichier absent
			if (File.Exists(test)==false)
			{
				//exit
				MessageBox.Show("Vous n'êtes pas autorisé à utiliser ce logiciel\n\nMerci de contacter xxxxxxx si ce comportement est anormal\n","Erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
				Button1Click(null,null);
				System.Environment.Exit(0);
			}
			//Fichier absent
			if (File.Exists("Plan.csv")==false)
			{
				//exit
				MessageBox.Show("Plan.csv manquant","Erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
				System.Environment.Exit(0);
			}
/*			if (File.Exists("vncviewer.exe")==false)
			{
				//exit
				MessageBox.Show("vncviewer.exe manquant","Erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
				System.Environment.Exit(0);
			}*/
			binfile=new FonctionFichierBinaireCLR("Plan.csv");

			//Objet dans collection
			//InitCollections();

			//Détection de la licence windows.
			RegistryKey Nkey = Registry.LocalMachine;
			RegistryKey valKey = Nkey.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion", false);
			line = valKey.GetValue("ProductId").ToString();
			valKey.Close();
			Nkey.Close();
			if (line!="55711-641-6046354-23889")
			{
				MessageBox.Show("Vous n'êtes pas autorisé à utiliser ce logiciel\n\nMerci de contacter xxxxx si ce comportement est anormal\n","Alerte",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				Button1Click(0, null);
				System.Environment.Exit(1);
			}

			button2.Visible=false;
			button3.Visible=false;
			button4.Visible=false;
			button6.Visible=false;
			button7.Visible=false;
			label3.Visible=false;
			label4.Visible=false;

			for (comp=0;comp<POSTESMAX;comp++)
			{
				LabelArray[comp]=new Label();
				LabelArray[comp].Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
				LabelArray[comp].Location = new Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+30);
				LabelArray[comp].Name = "label"+comp.ToString();
				LabelArray[comp].Size = new Size(70, 15);
				LabelArray[comp].TabIndex = comp*4;
				LabelArray[comp].Text = "label"+comp.ToString();
				LabelArray[comp].TextAlign = ContentAlignment.TopCenter;
				LabelArray[comp].Visible = false;
//				LabelArray[comp].Enabled = true;
				this.Controls.Add(LabelArray[comp]);

				RDArray[comp]=new RemoteDesktop();
				RDArray[comp].GetPassword = passw;
				RDArray[comp].AutoScroll = false;
				RDArray[comp].Location = new Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+30);
				RDArray[comp].Name = "remoteDesktop"+comp.ToString();
				RDArray[comp].TabIndex = comp*4+1;
				RDArray[comp].Visible = false;
				RDArray[comp].Enabled = true;
				RDArray[comp].ContextMenuStrip = contextMenuStrip;
				this.Controls.Add(RDArray[comp]);

				LabelArrayMap[comp] = new Label();
				LabelArrayMap[comp].Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
				LabelArrayMap[comp].Location = new Point(0, 30);
				LabelArrayMap[comp].Visible = false;
				LabelArrayMap[comp].TabIndex = comp*4+3;
				LabelArrayMap[comp].Size = new Size(70, 15);
				LabelArrayMap[comp].TextAlign = ContentAlignment.TopCenter;
				this.Controls.Add(this.LabelArrayMap[comp]);

				PictureBoxArray[comp]=new PictureBox();
				PictureBoxArray[comp].ImageLocation="computer.bmp";
				PictureBoxArray[comp].Location = new Point(0, 30);
				PictureBoxArray[comp].Size = new Size(70,52);
				PictureBoxArray[comp].SizeMode = PictureBoxSizeMode.StretchImage;
				PictureBoxArray[comp].Name = "pictureBox"+comp.ToString();
				PictureBoxArray[comp].TabIndex = comp*4+2;
				PictureBoxArray[comp].TabStop = false;
				PictureBoxArray[comp].Visible = false;
				this.Controls.Add(this.PictureBoxArray[comp]);
			}
			mode="";

			//Version prof
			nposte="";
			nsalle="";
			if (vprof==true)
			{
				redémarrerToolStripMenuItem.Visible=false;
				redémarrerToolStripMenuItem1.Visible=false;
				comboBox1.Visible=false;
				comboBox2.Visible=false;
				label1.Text="";
				while (!binfile.EndOfStream)
				{
					line = binfile.LireLigne();

					if (line.Length<2)
					{
						//Ligne vide ou trop courte ignoré
					}
					else if (line[0]=='/'&line[1]=='/')
					{
						//Contient commentaire, donc ligne ignoré
					}
					else if (line[0]==':'&line[1]!=':')//etage
					{
						//etage
					}
					else if (line[0]==':'&line[1]==':')//salle
					{
						nsalle=line.Substring(2);
					}
					else if (line.Length>0)//poste
					{
						comp = line.IndexOf(';');
						nposte=line.Substring(0,comp);
						if(nposte==poste_courant & nsalle!=null)
						{
							comboBox2.Items.Add(nsalle);
							comboBox2.SelectedItem=nsalle;
							label2.Text="                VNC MANAGER 3.1\n\nCette version apporte de nouvelles fonctions.\n\n" +
								"-prise de photo\n-prise de controle du poste\n\nHave fun !";
							break;
						}
					}
				}
				if (binfile.EndOfStream)
				{
					MessageBox.Show("Numéro de poste introuvable.\nLa salle ne contient peut être qu'un seul poste\n\nMerci de contacter xxxx si ce comportement est anormal","Erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
					System.Environment.Exit(1);
				}
			}
			//Version Admin
			else
			{
				comp=0;
				label2.Visible=false;
				button5.Visible=false;
				//MessageBox.Show(comp0.ToString());
				binfile.Seek0();
				while (!binfile.EndOfStream)
				{
					line = binfile.LireLigne();

					if (line.Length!=0)//Ligne vide ignoré
					{
						if (line[0]==':'&line[1]!=':')//étage
						{
							nsalle = line.Substring(1);
							comboBox1.Items.Add(nsalle);
						}
					}
				}
			}
//			this.ResumeLayout(false);
			binfile.Ferme();
		}
		private void MainForm_Resize(object sender, EventArgs e)
		{
			byte comp=0;
			int x,y;
			float ratio,a,b;

			if(this.Width<1024)
			{
				this.Width=1024;
			}
			if(this.Height<768)
			{
				this.Height=768;
			}
			button1.Location = new Point(this.Width-32, 1);
			button2.Location = new Point(this.Width-126, 1);
			button3.Location = new Point(this.Width-207, 1);
			button4.Location = new Point(this.Width-288, 1);
			button6.Location = new Point(this.Width-369, 1);
			button7.Location = new Point(this.Width-450, 1);
			progressBar1.Location = new Point(this.Width-543, 1);

			foreach(RemoteDesktop rd in RDArray)
			{
				if (rd!=null)
				{
					if(rd.Desktop!=null)
					{
						a=float.Parse(RDArray[comp].Desktop.Height.ToString());
						b=float.Parse(RDArray[comp].Desktop.Width.ToString());
						ratio = a/b;
				if(poste_fc==comp & (mode=="Fullscreen" | mode=="Control"))
				{
					ratio=1/ratio;
					x=this.Width-6;
					y=(int)(x/ratio);
					if(y>this.Height-59)
					{
						y=this.Height-59;
						x=(int)(y*ratio);
					}
					rd.Size = new Size(x,y);
					//rd.Visible=true;
				}
				else
				{
					LabelArray[comp].Location = new Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+25);
					rd.Location = new Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+25);
					rd.Size = new Size(this.Width/5,(int)(this.Width*ratio)/5);
				}
					}
				}
				comp++;
			}
/*			if(mode=="Fullscreen" | mode=="Control")
			{
					a=float.Parse(RDArray[poste_fc].Desktop.Width.ToString());
					b=float.Parse(RDArray[poste_fc].Desktop.Height.ToString());
					ratio = a/b;
			}*/
//replace la carte
			int aa,bb;
			aa=Width/170;
			bb=Height/150;
			for(comp=0;comp<nb_poste;comp++)
			{
				PictureBoxArray[comp].Location = new Point(aa*int.Parse(liste_poste[comp,1]),bb*int.Parse(liste_poste[comp,2])+30);
				LabelArrayMap[comp].Location = new Point(aa*int.Parse(liste_poste[comp,1]),bb*int.Parse(liste_poste[comp,2])+85);
			}
		}
		private void MainForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			button2.Visible=false;
			button3.Visible=false;
			button4.Visible=false;
			button6.Visible=false;
			button7.Visible=false;
			Déconnexion();
			binfile.Ferme();
		}
		//Mise à jour de la mosaique et de la liste des postes en ligne
		void MAJ()
		{
			int comp;
			for(comp=0;comp<nb_poste;comp++)
			{
				if(RDArray[comp].IsConnected==true)
				{
					RDArray[comp].Visible=true;
					liste_poste[comp,3]="1";
				}
				else
				{
					RDArray[comp].Visible=false;
					liste_poste[comp,3]="0";
				}
				LabelArray[comp].Visible=false;
			}
			etat_labels=false;
		}

		//Affiche la carte des postes.
		void CarteSalle()
		{
			int comp;

			if(salle_courante==null)
			{
				return;
			}

			if(mode=="Carte")
			{
				button6.BackColor=SystemColors.ActiveCaptionText;
				label3.Visible=false;
				label4.Visible=false;
				if(vprof==false)
				{
					comboBox1.Visible=true;
					comboBox2.Visible=true;
				}
				switch (mode_save)
				{
					case "Mosaique":
						button3.Visible=true;
						button4.Visible=true;
						button6.Visible=false;
						button7.Visible=false;
						for (comp=0;comp<nb_poste;comp++)
						{
							if(liste_poste[comp,3]=="1")
							{
								RDArray[comp].Visible = true;
								LabelArray[comp].Visible = false;
								etat_labels=false;
							}
							LabelArrayMap[comp].Visible = false;
							PictureBoxArray[comp].Visible = false;
						}
						break;
					case "Fullscreen":
						LabelArrayMap[poste_fc].BackColor = SystemColors.ActiveCaptionText;
						button3.Visible=true;
						button4.Visible=true;
						button6.Visible=true;
						button7.Visible=true;
						if(liste_poste[poste_fc,3]=="1")
						{
							RDArray[poste_fc].Visible = true;
							LabelArray[poste_fc].Visible = false;
							etat_labels=false;
						}
						for (comp=0;comp<nb_poste;comp++)
						{
							LabelArrayMap[comp].Visible = false;
							PictureBoxArray[comp].Visible = false;
						}
						break;
				}
				mode=mode_save;
				button2.Text="Carte de la salle";
			}
			else
			{
				mode_save=mode;
				mode="Carte";
				button2.Text="Retour";
				button3.Visible=false;
				button4.Visible=false;
				button6.Visible=false;
				button7.Visible=false;
				label3.Visible=true;
				comboBox1.Visible=false;
				comboBox2.Visible=false;

				if(mode_save=="Fullscreen")
				{
					label4.Visible=true;
					LabelArrayMap[poste_fc].BackColor = Color.Green;
				}

				for (comp=0;comp<nb_poste;comp++)
				{
					if(liste_poste[comp,3]=="1")
					{
						RDArray[comp].Visible = false;
						LabelArray[comp].Visible = false;
						etat_labels=false;
					}
					LabelArrayMap[comp].Visible = true;
					PictureBoxArray[comp].Visible = true;
				}
			}
//			MainForm_Resize(null,null);
/*
			temps= DateTime.Now.Ticks+10000000;
			while(DateTime.Now.Ticks<temps)
			{
				comp0=1-((float)(temps-DateTime.Now.Ticks)/(float)10000000);
				MessageBox.Show(comp0.ToString());
				for (comp=0;comp<nb_poste;comp++)
				{
					PictureBoxArray[comp].Location = new Point((int)(comp0*(float)int.Parse(liste_poste[comp,1])*10),(int)(comp0*(float)int.Parse(liste_poste[comp,2]))*10);
					PictureBoxArray[comp].Location = new Point(int.Parse(liste_poste[comp,1])*10,int.Parse(liste_poste[comp,2])*10);
					PictureBoxArray[comp].Visible = true;

					LabelArrayMap[comp].Location = new Point((int)(comp0*(float)int.Parse(liste_poste[comp,1])*10),(int)(comp0*(float)int.Parse(liste_poste[comp,2]))*10);
					LabelArrayMap[comp].Location = new Point(int.Parse(liste_poste[comp,1])*10,int.Parse(liste_poste[comp,2])*10);
					LabelArrayMap[comp].Visible = true;
					if(poste_courant==liste_poste[comp,0].ToUpper())
					{
						LabelArrayMap[comp].BackColor = Color.Red;
					}
				}
			}
*/
		}

		//Connexion des fenetres VNC
		void Connexion()
		{
			bool res;
//			float ratio,a,b;
			int comp=0;
			Ping EnvoiPing = new Ping();
			PingReply rep;

			MasqueTout();

			if(mode=="Carte")
			{
				etat_labels=true;
				CarteSalle();
			}
			else if(mode=="Fullscreen")
			{
				button2.Visible=false;
				button3.Visible=false;
				button4.Visible=false;
				button6.Visible=false;
				button7.Visible=false;
				//Test poste en ligne
				try
				{
					rep = EnvoiPing.Send(liste_poste[poste_fc,0],100);
					if(rep.Status== IPStatus.TimedOut)
					{
						res=false;
					}
					else
					{
						res=true;
					}
				}
				catch(Exception)
				{
					res=false;
				}
				if(RDArray[poste_fc].IsConnected==false & res)
				{
					//MessageBox.Show("En ligne");
					try
					{
						RDArray[poste_fc].Connect(liste_poste[poste_fc,0],true,true);
						res=true;
					}
					catch (VncProtocolException)
					{
						res=false;
					}
					if (res)
					{
/*								b=float.Parse(RDArray[comp].Desktop.Width.ToString());
						a=float.Parse(RDArray[comp].Desktop.Height.ToString());
						ratio = a/b;
						LabelArray[comp].Location = new Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+25);
						RDArray[comp].Location = new Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+25);
						RDArray[comp].Size = new Size(this.Width/5,(int)(this.Width/5*ratio));*/
						//RDArray[poste_fc].Visible=true;
						//RDArray[comp].Show();
						liste_poste.SetValue("1",poste_fc,3);
					}
					else//hors ligne retour mosaique
					{
						mode="Mosaique";
						Connexion();
						return;
					}
				}
				else if(res==false)//hors ligne retour mosaique
				{
					mode="Mosaique";
					Connexion();
					return;
				}
				RDArray[poste_fc].Visible=true;
				button2.Visible=true;
				button3.Visible=true;
				button4.Visible=true;
				button6.Visible=true;
				button7.Visible=true;
			}
			else if (mode=="Mosaique")
			{
				button2.Visible=false;
				button3.Visible=false;
				button4.Visible=false;
				button4.Text="Noms";
				button6.Visible=false;
				button7.Visible=false;

				progressBar1.Maximum=nb_poste;
				progressBar1.Value=0;
				progressBar1.Visible=true;
				for (comp=0;comp<nb_poste;comp++)
				{
					if(liste_poste[comp,0]!=null)
					{
						progressBar1.Value++;
						RDArray[comp].Name=liste_poste[comp,0];
						RDArray[comp].ContextMenuStrip.AccessibleName=liste_poste[comp,0];
						LabelArray[comp].Text = liste_poste[comp,0];
						LabelArrayMap[comp].Text = liste_poste[comp,0];
						if(poste_courant==liste_poste[comp,0].ToUpper())
						{
							LabelArrayMap[comp].BackColor = Color.Red;
						}
						else
						{
							LabelArrayMap[comp].BackColor = SystemColors.ActiveCaptionText;
							//Test poste en ligne
							try
							{
								rep = EnvoiPing.Send(liste_poste[comp,0],100);
								if(rep.Status== IPStatus.TimedOut)
								{
									res=false;
								}
								else
								{
									res=true;
								}
							}
							catch(Exception)
							{
								res=false;
							}
							if(RDArray[comp].IsConnected==false & res & liste_poste[comp,0]!=poste_courant)
							{
								//MessageBox.Show("En ligne");
								try
								{
									RDArray[comp].Connect(liste_poste[comp,0],true,true);
									res=true;
								}
								catch (VncProtocolException)
								{
									res=false;
								}
								if (res)
								{
	/*								b=float.Parse(RDArray[comp].Desktop.Width.ToString());
									a=float.Parse(RDArray[comp].Desktop.Height.ToString());
									ratio = a/b;
									LabelArray[comp].Location = new Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+25);
									RDArray[comp].Location = new Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+25);
									RDArray[comp].Size = new Size(this.Width/5,(int)(this.Width/5*ratio));*/
									RDArray[comp].Visible=true;
									//RDArray[comp].Show();
									liste_poste.SetValue("1",comp,3);
								}
								else
								{
									RDArray[comp].Visible=false;
									liste_poste.SetValue("0",comp,3);
								}
							}
							else if(res==false)//hors ligne
							{
								RDArray[comp].Visible=false;
								liste_poste.SetValue("0",comp,3);
							}
							else
							{
								RDArray[comp].Visible=true;
							}
						}
					}
				}
				progressBar1.Visible=false;
				button2.Visible=true;
				button3.Visible=true;
				button4.Visible=true;
				button6.Visible=false;
				button7.Visible=false;
			}
			MainForm_Resize(null,null);
		}
		//Mot de passe pour les fenetre VNC
		string passw()
		{
			return "";
		}

		//Déconnexion des fenetres VNC
		void Déconnexion()
		{
			int comp;

			progressBar1.Value=nb_poste;
			progressBar1.Visible=true;

//			SuspendLayout();
			foreach(Label lb in LabelArray)
			{
				lb.Visible=false;
			}
			foreach(RemoteDesktop rd in RDArray)
			{
				rd.Visible=false;
			}
			for(comp=0;comp<nb_poste;comp++)
			{
				progressBar1.Value--;

				if(RDArray[comp].IsConnected==true)
				{
					RDArray[comp].Disconnect();
				}
			}
			salle_courante=null;
			progressBar1.Visible=false;
//			ResumeLayout(false);
//			PerformLayout();
		}
		void MasqueTout()
		{
			int comp;
			for(comp=0;comp<POSTESMAX;comp++)
			{
				RDArray[comp].Visible=false;
				LabelArray[comp].Visible=false;
				LabelArrayMap[comp].Visible=false;
				PictureBoxArray[comp].Visible=false;
			}
		}
		void Statistique(bool debut)
		{
			//statistique
			if(File.Exists(@"\\dc1\stat$\"+Environment.UserName))
			{
				if(debut)
				{
					try{
						File.AppendAllText(@"\\dc1\stat$\"+Environment.UserName+".txt","\r\nDébut: "+DateTime.Now.ToString());
					}
					catch(Exception){}
				}
				else
				{
					try{
						File.AppendAllText(@"\\dc1\stat$\"+Environment.UserName+".txt"," Fin: "+DateTime.Now.ToString());
					}
					catch(Exception){}
				}
			}
		}
/*		void ImageBoxClick(object sender, EventArgs e)
		{
			if (poste_courant != ((PictureBox)sender).Text)
			{
				Process myProc;
				myProc = Process.Start("vncviewer.exe", "-connect " + ((PictureBox)sender).Text + " -password -viewonly");
				//			myProc.WaitForExit();
			}
		}*/
		//Affiche classes dans 2e menu
		void ComboBox1DropdownClosed(object sender, EventArgs e)
		{
			bool rec=false;
			int comp;
			string line,nposte;

			comboBox2.Items.Clear();
			if(comboBox1.SelectedItem==null)
			{
				return;
			}

			binfile=new FonctionFichierBinaireCLR("Plan.csv");

			binfile.Seek0();
			while (!binfile.EndOfStream)
			{
				line = binfile.LireLigne();

				if (line.Length!=0)//Ligne vide ignoré
				{
					if (line[0]=='/'&line[1]=='/')
					{
						//Contient commentaire, donc ligne ignoré
					}
					else if (line[0]==':'&line[1]!=':')//étage
					{
						nposte=line.Substring(1);
						if(nposte==comboBox1.SelectedItem.ToString())
						{
							rec=true;
						}
						else
						{
							rec=false;
						}
					}
					else if  (line[0]==':'&line[1]==':'&rec==true)//salle
					{
						comp = line.IndexOf(';');
						if(comp==-1)//pas trouvé donc pas d'alias
						{
							comp=line.Length;
						}
						nposte=line.Substring(2,comp-2);
						comboBox2.Items.Add(nposte);
					}
				}
			}
			binfile.Ferme();
		}
		//Affiche la mosaique de la salle sélectionné
		void ComboBox2DropdownClosed(object sender, EventArgs e)
		{
			bool rec;
			int comp=0, comp2;
			string nposte, line;

//			SuspendLayout();

			if(comboBox2.SelectedItem==null | (string)comboBox2.SelectedItem==salle_courante)
			{
				return;
			}
			if(nb_poste!=0)
			{
				Déconnexion();
			}
			salle_courante=comboBox2.SelectedItem.ToString();
			this.label1.Text = salle_courante;
			liste_poste=new string[POSTESMAX,4];

			//Recherche des postes de cette classe
			binfile=new FonctionFichierBinaireCLR("Plan.csv");
			binfile.Seek0();
			nb_poste=0;
			rec=false;
			while (!binfile.EndOfStream)
			{
				line = binfile.LireLigne();

				if (line.Length<2)
				{
					//Ligne vide ou contenant 1 caractère
				}
				else if ((line[0]=='/'&line[1]=='/')|(line[0]==':'&line[1]!=':'))
				{
					//Contient commentaire ou étage, donc ligne ignoré
				}
				else if (line[0]==':'&line[1]==':')//salle
				{
					nposte = line.Substring(2);

					//Bonne salle
					if (nposte==salle_courante)
					{
						rec=true;
					}
					else//Mauvaise salle
					{
						rec=false;
					}
				}
				else if (line.Length>=0 & rec & nb_poste<POSTESMAX)//suite traitement
				{
					//Section poste
					comp=0;
					comp2 = line.IndexOf(';');
					nposte = line.Substring(comp,comp2-comp);
					liste_poste.SetValue(nposte.ToUpper(),nb_poste,0);

					comp=comp2+1;
					comp2 = line.IndexOf(';',comp);
					nposte = line.Substring(comp,comp2-comp);
					liste_poste.SetValue(nposte.ToUpper(),nb_poste,1);

					comp=comp2+1;
					comp2 = line.Length;
					nposte = line.Substring(comp,comp2-comp);
					liste_poste.SetValue(nposte.ToUpper(),nb_poste,2);

					liste_poste.SetValue("0",nb_poste,3);
					nb_poste++;
				}
				else if (line.Length>=0 & rec & nb_poste==POSTESMAX)//suite traitement
				{
					MessageBox.Show("Il est, pour le moment, impossible d'afficher plus de "+POSTESMAX.ToString()+" postes par page avec ce logiciel\n\nSeul les 20 premiers seront visibles\n");
					break;
				}
			}

			//préparation de la mosaique
			mode="Mosaique";
			Connexion();
			binfile.Ferme();
//			ResumeLayout(false);
//			PerformLayout();
		}
		//A propos
		void Button1Click(object sender, EventArgs e)
		{
			MessageBox.Show("VNC Manager V3.1\n\nPour le lycée Jacques Prévert.\n\nContact xxxx\n\nC.L.R. 2012","A propos",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
		//Carte de la salle
		void Button2Click(object sender, EventArgs e)
		{
			CarteSalle();
		}
		//Rafraichir
		void Button3Click(object sender, EventArgs e)
		{
			Connexion();
		}
		//Affiche les labels sur les VNC ou Arêter plein écran/controle
		void Button4Click(object sender, EventArgs e)
		{
			byte comp;

			switch (mode)
			{
				case "Mosaique":
	//				SuspendLayout();
					etat_labels=!etat_labels;
					for (comp=0;comp<nb_poste;comp++)
					{
						if(liste_poste[comp,3]=="1")
						{
							LabelArray[comp].Visible=etat_labels;
							//LabelArray[comp].TabIndex;
							//RDArray[comp].TabIndex;
						}
					}
	//				ResumeLayout(false);
	//				PerformLayout();
					break;
				case "Control":
					button6.BackColor=SystemColors.ActiveCaptionText;
					button4.Text="Rétrécir";
					button3.Visible=true;
					button2.Visible=true;
					button1.Visible=true;
					if(vprof==false)
					{
						comboBox2.Visible=true;
						comboBox1.Visible=true;
					}
					RDArray[poste_fc].ContextMenuStrip=contextMenuStrip1;
					RDArray[poste_fc].SetInputMode(true);
					mode="Fullscreen";
					break;
				case "Fullscreen":
					RétrécirToolStripMenuItemClick(null,null);
					button4.Text="Noms";
					break;
			}
		}
		void VoirEnGrandToolStripMenuItemClick(object sender, EventArgs e)
		{
			int comp;
			ContextMenuStrip c;

			c=(ContextMenuStrip)((ToolStripMenuItem)sender).Owner;

			if (poste_courant != ((ToolStripMenuItem)sender).GetCurrentParent().AccessibleName | mode!="Fullscreen")
			{
//				Process myProc;
//				myProc = Process.Start("vncviewer.exe", "-connect " + ((Label)sender).Text + " -password -viewonly");
				for (comp=0;comp<nb_poste;comp++)
				{
					if(liste_poste[comp,0]==c.SourceControl.Name)
					{
						label_save=label1.Text;
						poste_fc=comp;
						label1.Text="Vu du poste "+c.SourceControl.Name;
						RDArray[comp].Location = new Point(0, 25);
						RDArray[comp].Visible = true;
						RDArray[comp].ContextMenuStrip = contextMenuStrip1;
					}
					else
					{
						RDArray[comp].Visible = false;
					}
					LabelArray[comp].Visible = false;
				}
				etat_labels=false;
				mode="Fullscreen";
				MainForm_Resize(sender,e);
				button4.Text="Rétrécir";
				button6.Visible=true;
				button7.Visible=true;
			}
		}
		void RétrécirToolStripMenuItemClick(object sender, EventArgs e)
		{
			int comp;

			for (comp=0;comp<nb_poste;comp++)
			{
				if(liste_poste[comp,3]=="1")
				{
					RDArray[comp].Visible = true;
					RDArray[comp].ContextMenuStrip = contextMenuStrip;
				}
				else
				{
					RDArray[comp].Visible = false;
				}
			}
			LabelArrayMap[poste_fc].BackColor = SystemColors.ActiveCaptionText;
			mode="Mosaique";
			label1.Text=label_save;
			RDArray[poste_fc].ContextMenuStrip = contextMenuStrip;
			MainForm_Resize(null,null);
			button6.Visible=false;
			button7.Visible=false;
		}
		
		void PrendreUnePhotoToolStripMenuItemClick(object sender, EventArgs e)
		{
			int comp;
			if(path==null | path=="")
			{
				folderBrowserDialog1.ShowDialog();
				path=folderBrowserDialog1.SelectedPath;
			}
			if(path!="")
			{
				for(comp=0;comp<32;comp++)
				{
					if (File.Exists(path+"\\"+liste_poste[poste_fc,0]+"_"+comp.ToString()+".bmp")==false)
					{//prend la photo
						Bitmap bm=new Bitmap(RDArray[poste_fc].Width,RDArray[poste_fc].Height);
						Rectangle t=new Rectangle(0,0,RDArray[poste_fc].Width,RDArray[poste_fc].Height);
						RDArray[poste_fc].DrawToBitmap(bm,t);
						bm.Save(path+"\\"+liste_poste[poste_fc,0]+"_"+comp.ToString()+".bmp");
						break;
					}
				}
			}
		}

		void RedémarrerToolStripMenuItemClick(object sender, EventArgs e)
		{
			ContextMenuStrip c;
			Process myProc;

			c=(ContextMenuStrip)((ToolStripMenuItem)sender).Owner;
			if(((RemoteDesktop)(c.SourceControl)).IsConnected==true)
			{
				((RemoteDesktop)(c.SourceControl)).Disconnect();
			}
			((RemoteDesktop)(c.SourceControl)).Visible=false;
			myProc = Process.Start("shutdown", " -r -t 0 -m \\\\" + c.SourceControl.Name);
			RétrécirToolStripMenuItemClick(null,null);
		}
		//Démarrer
		void Button5Click(object sender, EventArgs e)
		{
			button5.Visible=false;
			label2.Visible=false;
			ComboBox2DropdownClosed(null,null);
		}
		void ContrôleToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(mode=="Control")
			{
				Button4Click(sender,e);
			}
			else
			{
				button4.Text="Retour";
				button6.BackColor=Color.Red;
				button3.Visible=false;
				button2.Visible=false;
				button1.Visible=false;
				comboBox2.Visible=false;
				comboBox1.Visible=false;
				RDArray[poste_fc].ContextMenuStrip=null;
				RDArray[poste_fc].SetInputMode(false);
				mode="Control";
			}
		}
}




public class FonctionCLR
{
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
				retour=line.Substring(oldcomp,comp-oldcomp);
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
//		BinaryWriter fw;

	public FonctionFichierBinaireCLR(string str)
	{
		fs = new FileStream(str,FileMode.OpenOrCreate, FileAccess.Read);
		fr = new BinaryReader(fs,Encoding.ASCII);
//			fw = new BinaryWriter(fs,Encoding.ASCII);
	}
	public void Ferme()
	{
		fr.Close();
//			fw.Close();
		fs.Close();
	}
	/*		public void EcrireLigne(string str)
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
		}*/
	public void Seek0()
	{
		fs.Seek(0,0);
		EndOfStream=false;
	}
	public string LireLigne()
	{
		byte by=0;
		bool ligne=false;
		char[] chr = new char[2048];
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
