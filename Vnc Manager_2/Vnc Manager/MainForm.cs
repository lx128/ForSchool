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
using System.Windows.Forms;

using System.Net.NetworkInformation;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using VncSharp;
using System.Drawing;
using System.Threading;

namespace Vnc_Manager
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		//Début programme
		private const byte NBELEMENT = 19;
		private const byte NBMENU1 = 20;
		private const byte NBMENU2 = 11;
		private const byte NBMENU3 = 2;

		private Label[] MyLabelArray = new Label[NBELEMENT];
		private PictureBox[] MyPictureBoxArray = new PictureBox[NBELEMENT];
		private ToolStripMenuItem[]
		MyTSM1=new ToolStripMenuItem[NBMENU1],
		MyTSM2=new ToolStripMenuItem[NBMENU2],
		MyTSM3=new ToolStripMenuItem[NBMENU3];
		private bool vprof = false;
		private byte nbposteaff=0;
		private string currentsalle="", ceposte;
		Form1 MosaiqueSalle;

		private string[,] établissement;
		FonctionCLR myfunc=new FonctionCLR();
		FonctionFichierBinaireCLR binfile;

		private byte nb_poste=0;
		private bool etat_des_postes=true;
		public bool actif=false;
		private string[] liste_poste = new string[NBELEMENT];
		private RemoteDesktop[] MyRDArray= new RemoteDesktop[NBELEMENT];
		private Label[] LabelArray= new Label[NBELEMENT];

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//

			char[] str= new char[16];
			int comp=0, comp0=0;
			string nsalle="", line, étage;

			ceposte=System.Environment.MachineName.ToUpper();
//nsalle="B208";
//ceposte="B01-01";

			//Fichier absent
			if (System.IO.File.Exists("Plan.csv")==false)
			{
				//exit
				MessageBox.Show("Plan.csv manquant","Erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
				System.Environment.Exit(0);
			}
			if (System.IO.File.Exists("vncviewer.exe")==false)
			{
				//exit
				MessageBox.Show("vncviewer.exe manquant","Erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
				System.Environment.Exit(0);
			}
			binfile=new FonctionFichierBinaireCLR("Plan.csv");

			//Objet dans collection
			InitCollections();

			//Détection de la licence windows.
			RegistryKey Nkey = Registry.LocalMachine;
			RegistryKey valKey = Nkey.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion", false);
			line = valKey.GetValue("ProductId").ToString();
			valKey.Close();
			Nkey.Close();
			if (line=="55711-641-6046354-23889")
			{
				MessageBox.Show("Vous n'êtes pas autorisé à utiliser ce logiciel\n\nMerci de contacter xxxx si ce comportement est anormal\n\nID du poste "+line,"Alerte",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				AProposToolStripMenuItemClick(0, null);
				System.Environment.Exit(1);
			}

//Version prof
			if (vprof==true)
			{
				//this.Etage1ToolStripMenuItem.Visible=false;
				//this.Etage2ToolStripMenuItem.Visible=false;
				//this.RdcToolStripMenuItem.Visible=false;
	
				comp = ceposte.IndexOf('-');
				if (comp != -1)
				{
					ceposte.CopyTo(0,str,0,comp);
					nsalle = new string(str,0,comp);//La seule solution pour convertir char* en string
					nsalle = nsalle.ToUpper();
					DessineSalle(nsalle);
				}
				else
				{
					//this.label1.Text = "Numéro de poste non conforme.";
					MessageBox.Show("Numéro de poste non conforme.\n\nMerci de contacter xxxx si ce comportement est anormal","Erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
					System.Environment.Exit(1);
				}
				if(nbposteaff<2)
				{
					MessageBox.Show("Cette salle ne contient qu'un seul poste ou n'est pas prévu.\n\nMerci de contacter xxxx si ce comportement est anormal","Alerte",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
					System.Environment.Exit(1);
				}
				this.Show();
				MessageBox.Show("La mosaique est enfin disponible dans le menu en haut à gauche.","Aide",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
//Version Admin
			else
			{
				comp0=0;
				while (!binfile.EndOfStream)
				{
					line = binfile.LireLigne();

					if (line.Length!=0)//Ligne vide ignoré
					{
						if (line[0]=='/'&line[1]=='/')
						{
							//Contient commentaire, donc ligne ignoré
						}
						else if (line[0]==':'&&line[1]!=':')//étage
						{
							line.CopyTo(1,str,0,line.Length-1);
							nsalle = new string(str,0,line.Length-1);//La seule solution pour convertir char* en string
							étage=nsalle;
						}
						else if (line[0]==':'&&line[1]==':')//Salle
						{
//								nsalle.CopyTo(0,str,0,comp);
//								nsalle = new string(str,0,comp);//La seule solution pour convertir char* en string
							comp0+=1;
								/*if(Array.BinarySearch(établissement, nsalle)==0)
								{
									//établissement.SetValue(étage, nsalle);
								}*/
						}
					}
				}
//MessageBox.Show(comp0.ToString());
				établissement = new string[comp0,2];
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
							
							line.CopyTo(1,str,0,line.Length-1);
							nsalle = new string(str,0,line.Length-1);//La seule solution pour convertir char* en string
							comboBox1.Items.Add(nsalle);
/*							switch (line)
							{
								case ":RDC;X;Y"://RDC
										etage=0;
								break;
								case ":PREMIER;;"://egal à premier
										etage=1;
								break;
								case ":DEUXIEME;;"://egal à deuxieme
										etage=2;
								break;
							}*/
						}
/*						else if (line[0]==':'&line[1]==':')//salle
						{
							//Section poste
							comp = line.IndexOf('-');
	//System.Windows.Forms.MessageBox.Show(comp.ToString());
							line.CopyTo(0,str,0,comp);
							nsalle = new string(str,0,comp);//La seule solution pour convertir char* en string
							nsalle = nsalle.ToUpper();
	
							//changement du menu
							if (nsalle!=nsallesave)//pour eviter de recommencer la même chose
							{
								if(etage==0)
								{
									MyTSM3[comp0].Text=nsalle;
									MyTSM3[comp0].Visible=true;
									MyTSM3[comp0].Click += new System.EventHandler(MyTSM1Click);
									comp0++;
								}
								else if(etage==1)
								{
									MyTSM1[comp1].Text=nsalle;
									MyTSM1[comp1].Visible=true;
									MyTSM1[comp1].Click += new System.EventHandler(MyTSM1Click);
									comp1++;
								}
								else if(etage==2)
								{
									MyTSM2[comp2].Text=nsalle;
									MyTSM2[comp2].Visible=true;
									MyTSM2[comp2].Click += new System.EventHandler(MyTSM1Click);
									comp2++;
								}
							}
							nsallesave=nsalle.ToString();
						}*/
					}
				}
			}
			this.Resize += new EventHandler(MainForm_Resize);
			FormClosing += MainForm_Resize;

			SuspendLayout();
			for (comp=0;comp<NBELEMENT;comp++)
			{
				LabelArray[comp]=new Label();
				LabelArray[comp].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				LabelArray[comp].Location = new System.Drawing.Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5);
				LabelArray[comp].Name = "label"+comp.ToString();
				LabelArray[comp].Size = new System.Drawing.Size(70, 15);
				LabelArray[comp].TabIndex = LabelArray[comp].TabIndex;
				LabelArray[comp].Text = "label"+comp.ToString();
				LabelArray[comp].TextAlign = System.Drawing.ContentAlignment.TopLeft;
				LabelArray[comp].Visible = false;
				LabelArray[comp].Enabled = false;
				Controls.Add(LabelArray[comp]);

				MyRDArray[comp]=new RemoteDesktop();
				MyRDArray[comp].GetPassword = passw;
				MyRDArray[comp].AutoScroll = false;
				MyRDArray[comp].Location = new System.Drawing.Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+24);
				MyRDArray[comp].Name = "remoteDesktop"+comp.ToString();
				MyRDArray[comp].TabIndex = 0;
				MyRDArray[comp].Visible = false;
				MyRDArray[comp].Enabled = false;
				this.Controls.Add(MyRDArray[comp]);
			}

			ResumeLayout(false);
			PerformLayout();

			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.MainForm_FormClosing );
		}
		private void MainForm_Resize(object sender, System.EventArgs e)
		{
			byte comp=0;
			float ratio,a,b;

			foreach(RemoteDesktop rd in MyRDArray)
			{
				if (rd!=null)
				{
					if(rd.Desktop!=null)
					{
						b=float.Parse(rd.Desktop.Width.ToString());
						a=float.Parse(rd.Desktop.Height.ToString());
						ratio = a/b;
						LabelArray[comp].Location = new System.Drawing.Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+25);
						rd.Location = new System.Drawing.Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+25);
						rd.Size = new System.Drawing.Size(this.Width/5,(int)((int.Parse(this.Width.ToString())/5)*ratio));
					}
				}
				comp++;
			}
		}
        private string passw()
        {
        	return "";
        }
		private void MainForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			Thread th=new Thread(new ThreadStart (FermetureForm));
			th.Start();
			th.Join();

			try{
				MosaiqueSalle.Close();
			}
			catch(System.Exception){}
/*			try{
			System.IO.File.AppendAllText(@"\\pdc\stat$\"+System.Environment.UserName+".txt"," Fin MainForm: "+DateTime.Now.ToString());
			}
			catch(System.Exception){}*/
		}
		private void FermetureForm()
		{
			Form2 InfoBox;
			InfoBox= new Form2();
			InfoBox.Show();
		}

		private void DessineSalle(string cur_salle)
		{
			FonctionCLR myfunc=new FonctionCLR();
			FonctionFichierBinaireCLR binfile=new FonctionFichierBinaireCLR("Plan.csv");
			char[] str= new char[16];
			int comp, posx, posy;
			string nposte, line, sposx, sposy;

			CacheTousPostes();
			currentsalle=cur_salle;
			nbposteaff=0;

			this.label1.Text = "Carte de la salle "+cur_salle;
			while (!binfile.EndOfStream)
			{
				line = binfile.LireLigne();
				line = line.ToLower();

				if (line.Length==0)//Ligne vide ignoré
				{
				}
				else if ((line[0]==';'&line[1]==';'))
				{
						//vide, donc ligne ignoré
				}
				else if ((line[0]=='/'&line[1]=='/')|line[0]==':')
				{
						//Contient commentaire ou étage, donc ligne ignoré
				}
				else//suite traitement
				{
					//Section poste
					comp = line.IndexOf('-');
					line.CopyTo(0,str,0,comp);
					nposte = new string(str,0,comp);//La seule solution pour convertir char* en string
					nposte = nposte.ToUpper();

					//Bonne salle
					if (nposte==cur_salle)
					{
						myfunc.element(line,0,out nposte,';');

						//PosX
						myfunc.element(line,1,out sposx,';');
						posx = int.Parse(sposx);
						comp=comp+3;

						//PosY
						myfunc.element(line,2,out sposy,';');
						posy = int.Parse(sposy);

						AjoutePoste(nposte, posx, posy);
					}
				}
			}
			binfile.Ferme();
		}
		private void InitCollections()
		{
			byte cmp;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

			for(cmp=0;cmp<NBELEMENT;cmp++)
			{
				MyLabelArray[cmp]=new Label();
				MyLabelArray[cmp].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				MyLabelArray[cmp].Name = "label"+cmp.ToString();
				MyLabelArray[cmp].TabIndex = 0;
				MyLabelArray[cmp].TextAlign = System.Drawing.ContentAlignment.TopCenter;
				MyLabelArray[cmp].Click += new System.EventHandler(LabelClick);
				MyLabelArray[cmp].Visible = false;
				MyLabelArray[cmp].Enabled = true;
				Controls.Add(MyLabelArray[cmp]);


				MyPictureBoxArray[cmp]=new PictureBox();
				MyPictureBoxArray[cmp].Image = ((System.Drawing.Image)(resources.GetObject("computer-icon_small")));
				MyPictureBoxArray[cmp].Name = "pictureBox"+cmp.ToString();
				MyPictureBoxArray[cmp].SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
				MyPictureBoxArray[cmp].TabIndex = 0;
				MyPictureBoxArray[cmp].Click += new System.EventHandler(ImageBoxClick);
				MyPictureBoxArray[cmp].Visible = false;
				MyPictureBoxArray[cmp].Enabled = true;
				Controls.Add(MyPictureBoxArray[cmp]);
			}

/*			for(cmp=0;cmp<NBMENU1;cmp++)
			{
				MyTSM1[cmp]=new ToolStripMenuItem();
				MyTSM1[cmp].Name = "s"+cmp.ToString()+"ToolStripMenuItem1";
				MyTSM1[cmp].Size = new System.Drawing.Size(100, 22);
				MyTSM1[cmp].Visible = false;
				MyTSM1[cmp].Enabled = true;
				this.Etage1ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {MyTSM1[cmp]});
			}
			for(cmp=0;cmp<NBMENU2;cmp++)
			{
				MyTSM2[cmp]=new ToolStripMenuItem();
				MyTSM2[cmp].Name = "s"+cmp.ToString()+"ToolStripMenuItem2";
				MyTSM2[cmp].Size = new System.Drawing.Size(100, 22);
				MyTSM2[cmp].Visible = false;
				MyTSM2[cmp].Enabled = true;
				this.Etage2ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {MyTSM2[cmp]});
			}
			for(cmp=0;cmp<NBMENU3;cmp++)
			{
				MyTSM3[cmp]=new ToolStripMenuItem();
				MyTSM3[cmp].Name = "s"+cmp.ToString()+"ToolStripMenuItem3";
				MyTSM3[cmp].Size = new System.Drawing.Size(100, 22);
				MyTSM3[cmp].Visible = false;
				MyTSM3[cmp].Enabled = true;
				this.RdcToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {MyTSM3[cmp]});
			}*/
		}
		private void CacheTousPostes()
		{
			byte cmp;
			for (cmp=0;cmp<NBELEMENT;cmp++)
			{
				MyLabelArray[cmp].Visible = false;
				MyPictureBoxArray[cmp].Visible = false;
			}
		}

		private void AjoutePoste(string nposte,int posx,int posy)
		{
			MyPictureBoxArray[nbposteaff].Location = new System.Drawing.Point(posx*6, posy*6);
			MyPictureBoxArray[nbposteaff].SizeMode = PictureBoxSizeMode.StretchImage;
			MyPictureBoxArray[nbposteaff].TabStop = false;
			MyPictureBoxArray[nbposteaff].Visible = true;
			MyPictureBoxArray[nbposteaff].Size = new System.Drawing.Size(70, 60);
			MyPictureBoxArray[nbposteaff].Text = nposte.ToUpper();

			MyLabelArray[nbposteaff].Visible = true;
			MyLabelArray[nbposteaff].Location = new System.Drawing.Point(posx*6, posy*6+60);
			MyLabelArray[nbposteaff].Text = nposte.ToUpper();
			MyLabelArray[nbposteaff].Size = new System.Drawing.Size(70, 15);
			if(ceposte==nposte.ToUpper())
			{
				MyLabelArray[nbposteaff].BackColor = Color.Red;
			}

			nbposteaff++;
		}

		void MosaiqueToolStripMenuItemClick(object sender, EventArgs e)
		{
			string[] poste= new string[NBELEMENT];
			byte nbposte=0,comp=0;

			//mosaiqueToolStripMenuItem.Enabled=false;
			if(MosaiqueSalle==null)
			{
				if (currentsalle.Length!=0 & nbposteaff!=0)
				{
					foreach (Label thislabel in MyLabelArray)
					{
						if (thislabel.Visible==true)
						{
							poste[comp]=thislabel.Text;
							comp++;
						}
					}
					nbposte=comp;
					MosaiqueSalle=new Form1(poste);
					//MosaiqueSalle.NbPoste(poste);
					MosaiqueSalle.Show();
					//Thread th=new Thread(new ThreadStart (MosaiqueSalle.Mosaique));
					//th.Start();
					MosaiqueSalle.Mosaique();
				}
			}
			//mosaiqueToolStripMenuItem.Enabled=true;
		}
		void MosaiqueThread()
		{
			/*Form1 MosaiqueSalle= new Form1(poste);
			MosaiqueSalle.Show();
			Thread th=new Thread(new ThreadStart (MosaiqueSalle.Mosaique));
			th.Start();
			MosaiqueSalle.Mosaique();*/
		}
		void ImageBoxClick(object sender, EventArgs e)
		{
			if (ceposte != ((PictureBox)sender).Text)
			{
				Process myProc;
				myProc = Process.Start("vncviewer.exe", "-connect " + ((PictureBox)sender).Text + " -password -viewonly");
	//			myProc.WaitForExit();
			}
		}
		void AProposToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			System.Windows.Forms.MessageBox.Show("VNC Manager V2.0\n\nPour le lycée Jacques Prévert.\n\nContact xxxx\n\nC.L.R. 2011","A propos",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
		void ComboBox1DropdownClosed(object sender, System.EventArgs e)
		{
			string line,nposte;
			char[] str= new char[64];
			bool rec=false;

			comboBox2.Items.Clear();
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
						line.CopyTo(1,str,0,line.Length-1);
						nposte = new string(str,0,line.Length-1);//La seule solution pour convertir char* en string
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
						line.CopyTo(2,str,0,line.Length-2);
						nposte = new string(str,0,line.Length-2);//La seule solution pour convertir char* en string
						comboBox2.Items.Add(nposte);
					}
				}
			}
		}
		void ComboBox2DropdownClosed(object sender, System.EventArgs e)
		{
			DessineSalle(comboBox2.SelectedItem.ToString());
		}
		void LabelClick(object sender, EventArgs e)
		{
			if (ceposte != ((Label)sender).Text)
			{
				Process myProc;
				myProc = Process.Start("vncviewer.exe", "-connect " + ((Label)sender).Text + " -password -viewonly");
	//			myProc.WaitForExit();
			}
		}
		void MyTSM1Click(object sender, System.EventArgs e)
		{
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			System.Windows.Forms.MessageBox.Show("VNC Manager V2.0\n\nPour le lycée Jacques Prévert.\n\nContact xxxx\n\nC.L.R. 2011","A propos",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
		void Form1(string [] poste)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			byte comp=0;

			this.Resize += new EventHandler(Form1_Resize);
			FormClosing += Form1_FormClosing;

			SuspendLayout();
			NbPoste(poste);
			for (comp=0;comp<NBELEMENT;comp++)
			{
				LabelArray[comp]=new Label();
				LabelArray[comp].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				LabelArray[comp].Location = new System.Drawing.Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5);
				LabelArray[comp].Name = "label"+comp.ToString();
				LabelArray[comp].Size = new System.Drawing.Size(70, 15);
				LabelArray[comp].TabIndex = LabelArray[comp].TabIndex;
				LabelArray[comp].Text = "label"+comp.ToString();
				LabelArray[comp].TextAlign = System.Drawing.ContentAlignment.TopLeft;
				LabelArray[comp].Visible = false;
				LabelArray[comp].Enabled = false;
				Controls.Add(LabelArray[comp]);

				MyRDArray[comp]=new RemoteDesktop();
				MyRDArray[comp].GetPassword = passw;
				MyRDArray[comp].AutoScroll = false;
				MyRDArray[comp].Location = new System.Drawing.Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+24);
				MyRDArray[comp].Name = "remoteDesktop"+comp.ToString();
				MyRDArray[comp].TabIndex = 0;
				MyRDArray[comp].Visible = false;
				MyRDArray[comp].Enabled = false;
				this.Controls.Add(MyRDArray[comp]);
			}

			ResumeLayout(false);
			PerformLayout();
		}
		public void NbPoste(string[] poste)
		{
			progressBar1.Minimum=0;
			progressBar1.Maximum=0;
			liste_poste = (string[])poste.Clone();
			foreach (string this_poste in poste)
			{
				if (this_poste!=null)
				{
					progressBar1.Maximum++;
				}
			}
		}
		public void Mosaique()
		{
			bool res;
			byte comp=0;
			float ratio,a,b;
			Ping EnvoiPing = new Ping();
			PingReply rep;

			//Statistique(true);

			actif=true;
			label1.Visible=true;
        	label1.Show();
        	progressBar1.Visible=true;
        	SuspendLayout();

       		progressBar1.Value=0;
        	foreach (string this_poste in liste_poste)
        	{
        		if(this_poste!=null)
        		{
	        		progressBar1.Value+=1;
					//Test poste en ligne
					try
					{
						rep = EnvoiPing.Send("M3",100);
						res=true;
					}
					catch(Exception)
					{
						res=false;
					}
					if (res & this_poste!=System.Environment.MachineName.ToUpper())
					{
						//MessageBox.Show("En ligne");
						if (Connexion(this_poste,comp)==true)
						{
							b=float.Parse(MyRDArray[comp].Desktop.Width.ToString());
							a=float.Parse(MyRDArray[comp].Desktop.Height.ToString());
							ratio = a/b;
							MyRDArray[comp].Show();
							MyRDArray[comp].Name=this_poste;
							LabelArray[comp].Text = this_poste;
							LabelArray[comp].Enabled = true;
							LabelArray[comp].Visible = false;
						}
						else
						{
							LabelArray[comp].Enabled = false;
							MyRDArray[comp].Visible=false;
						}
					}
					else
					{
						LabelArray[comp].Enabled = false;
						MyRDArray[comp].Visible=false;
					}
        		}
        	}
			label1.Visible=false;
			progressBar1.Visible=false;
			Form1_Resize(0,null);
			ResumeLayout(false);
			PerformLayout();
		}
		private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
		{
//			label2.Visible=true;
			progressBar1.Visible=true;
			Ping EnvoiPing = new Ping();
			PingReply rep;

     	  	SuspendLayout();
			foreach(Label lb in LabelArray)
			{
				lb.Visible=false;
			}
			foreach(RemoteDesktop rd in MyRDArray)
			{
				rd.Visible=false;
			}
			foreach(RemoteDesktop rd in MyRDArray)
			{
				progressBar1.Value-=1;

				if(rd.IsConnected==true)
				{
//					rep = EnvoiPing.Send(rd.Name,100);
//					if (rep.Status == IPStatus.Success & rd.Name!=System.Environment.MachineName.ToUpper())
//					{
							rd.Disconnect();
//					}
				}
			}
//			label2.Visible=false;
			ResumeLayout(false);
			PerformLayout();
			//Statistique(false);
			actif=false;
		}

		private void Form1_Resize(object sender, System.EventArgs e)
		{
			byte comp=0;
			float ratio,a,b;

			foreach(RemoteDesktop rd in MyRDArray)
			{
				if (rd!=null)
				{
					if(rd.Desktop!=null)
					{
						b=float.Parse(rd.Desktop.Width.ToString());
						a=float.Parse(rd.Desktop.Height.ToString());
						ratio = a/b;
						LabelArray[comp].Location = new System.Drawing.Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+25);
						rd.Location = new System.Drawing.Point(comp%5*this.Width/5+comp%5, comp/5*this.Height/5+25);
						rd.Size = new System.Drawing.Size(this.Width/5,(int)((int.Parse(this.Width.ToString())/5)*ratio));
					}
				}
				comp++;
			}
		}
		private bool Connexion(string poste, byte pos)
		{
			try
			{
				MyRDArray[pos].Connect(poste.ToUpper(),true,true);
			}
			catch (VncProtocolException)
			{
				//MessageBox.Show("Poste "+poste+" hors ligne");
				return false;
			}
			return true;
		}

		void RafraichirToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Form1_FormClosing(null,null);
			this.Mosaique();
		}

		void NomDesPostesToolStripMenuItemClick(object sender, EventArgs e)
		{
			SuspendLayout();
			foreach (Label ce_label in LabelArray)
			{
				if(ce_label.Enabled==true)
				{
					ce_label.Visible=etat_des_postes;
				}
			}
			etat_des_postes=!etat_des_postes;
			ResumeLayout(false);
			PerformLayout();
		}
/*		void Statistique(bool debut)
		{
			//statistique
/*			if(System.IO.File.Exists(@"\\pdc\stat$\"+System.Environment.UserName))
			{
			}*//*
			if(debut)
			{
				try{
				System.IO.File.AppendAllText(@"\\pdc\stat$\"+System.Environment.UserName+".txt","\r\nDébut: "+DateTime.Now.ToString());
				}
				catch(System.Exception){}
			}
			else
			{
				try{
				System.IO.File.AppendAllText(@"\\pdc\stat$\"+System.Environment.UserName+".txt"," Fin Form1   : "+DateTime.Now.ToString());
				}
				catch(System.Exception){}
			}
		}*/
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
