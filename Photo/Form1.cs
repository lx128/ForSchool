using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace WinFormCharpWebCam
{
    //Design by Pongsakorn Poosankam
    public partial class mainWinForm : Form
    {
		byte nom,prenom1,prenom2,prenom3,classe=0, eleve_courant=0;
       	string[] liste_nom;
		FonctionCLR myCLR = new FonctionCLR();
		FonctionFichierBinaireCLR siecle= new FonctionFichierBinaireCLR("siecle.csv");
		bool webcam_on=false, finclasse=false;
        WebCam webcam;

		public mainWinForm()
        {
            InitializeComponent();
        }
		public void mainWinForm_Closing(object sender, EventArgs e)
        {
			siecle.Ferme();
			webcam.Stop();
        }
        private void mainWinForm_Load(object sender, EventArgs e)
        {
			string line, str;
			int ret=0;
			byte comp;

			bntCapture.Enabled=false;
			bntSave.Enabled=false;
			button1.Enabled=false;
			webcam = new WebCam();

			webcam.InitializeWebCam(ref imgVideo);

	    	if (!File.Exists("siecle.csv"))
		    {
				MessageBox.Show("Fichier siecle.csv introuvable.\nCe fichier est une extraction de Siecle (anciennement sconet) avec les champs:\n" +
			                "Nom	Prénom 1	Division");
				return;
			}
			line=siecle.LireLigne();

			//test du séparateur
			if (line.IndexOf(';')==-1)
			{
				siecle.Ferme();
				MessageBox.Show("Le fichier siecle.csv doit avoir ; pour séparateur de colone.\n" +
				                "Il doit aussi contenir Nom, Prénom 1, Prénom 2, Prénom 3, Code Structure");
				return;
			}

			for(comp=0;-1!=myCLR.element(line,comp, out str, ';');comp++)
			{
				switch (str)
				{
				case "Nom":
				case "NOM":
					nom=comp;
					ret=ret+1;
					break;
				case "Prénom":
				case "PRENOM":
					prenom1=comp;
					ret=ret+1;
					break;
				case "Prénom 2":
				case "PRENOM2":
					prenom2=comp;
					ret=ret+1;
					break;
				case "Prénom 3":
				case "PRENOM3":
					prenom3=comp;
					ret=ret+1;
					break;
				case "Code Structure":
				case "CLASSES":
					classe=comp;
					ret=ret+1;
					break;
				}
			}
			if(ret<5)
			{
				siecle.Ferme();
				MessageBox.Show("Le fichier siecle.csv ne contient pas tout les champs ou la première ligne ne les présente pas.\n" +
				                "Il doit contenir Nom, Prénom 1, Prénom 2, Prénom 3, Code Structure");
				return;
			}
			while(siecle.EndOfStream!=true)
			{
				line = siecle.LireLigne();
				myCLR.element(line,classe,out str,';');

				if (comboBox1.FindString(str)==-1)
				{
					comboBox1.Items.Add(str);
				}
				comp++;
			}
        }

        private void bntStart_Click(object sender, EventArgs e)
        {
            webcam.Start();
            webcam.Continue();
            webcam_on=true;
        }

        private void bntStop_Click(object sender, EventArgs e)
        {
            webcam.Stop();
            webcam_on=false;
        }

        private void bntContinue_Click(object sender, EventArgs e)
        {
            webcam.Continue();
        }

        private void bntCapture_Click(object sender, EventArgs e)
        {
        	imgCapture.Image=imgVideo.Image;

            imgCapture.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            imgCapture.Width=imgCapture.Image.Width;
            imgCapture.Height=imgCapture.Image.Height;
            if(!finclasse)
            {
				button1.Enabled=true;
            }
            bntSave.Enabled=true;
        }

        private void bntSave_Click(object sender, EventArgs e)
        {
            Helper.SaveImageCapture(imgCapture.Image);
        }

        private void bntVideoFormat_Click(object sender, EventArgs e)
        {
            webcam.ResolutionSetting();
        }

        private void bntVideoSource_Click(object sender, EventArgs e)
        {
            webcam.AdvanceSetting();
        }
        
        void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
        {
        	int comp=0;
        	string line,str="";
        	string[] liste=new string[64];

        	siecle.Seek0();
			line = siecle.LireLigne();
			liste_nom=new string[64];

			if(comboBox1.SelectedItem.ToString()=="")
			{
				bntCapture.Enabled=false;
				button1.Enabled=false;
				return;
			}
			while(siecle.EndOfStream!=true)
			{
				line = siecle.LireLigne();
				myCLR.element(line,classe,out str,';');

				if (comboBox1.SelectedItem.ToString()==str)
				{
					myCLR.element(line,nom,out str,';');
					liste_nom[comp]= str;
					myCLR.element(line,prenom1,out str,';');
					liste_nom[comp]+= "_";
					liste_nom[comp]+= str;
					myCLR.element(line,prenom2,out str,';');
					if(str.Count()!=0)
					{
						liste_nom[comp]+= " ";
						liste_nom[comp]+= str;
					}
					myCLR.element(line,prenom3,out str,';');
					if(str.Count()!=0)
					{
						liste_nom[comp]+= " ";
						liste_nom[comp]+= str;
					}
					comp++;
				}
			}
			/*IEnumerable<string> query = from word in liste_nom orderby word.Substring(0, 1) ascending select word;
            foreach (string st in query)
            {
                MessageBox.Show(st);
            }*/
			if (!Directory.Exists(Application.StartupPath+"\\"+comboBox1.SelectedItem.ToString()))
		    {
		    	Directory.CreateDirectory(Application.StartupPath+"\\"+comboBox1.SelectedItem.ToString());
		    }
			if(!webcam_on)
			{
				bntStart_Click(null,null);
			}

			eleve_courant=0;
			bntCapture.Enabled=true;
			EleveSuivant();
        }
        private void EleveSuivant()
        {
        	if(!File.Exists(Application.StartupPath+"\\"+comboBox1.SelectedItem.ToString()+"\\"+liste_nom[eleve_courant]+".jpg"))
        	{
        		label2.Text=liste_nom[eleve_courant];
				button1.Enabled=true;
				finclasse=false;
        	}
        	else
        	{
        		eleve_courant++;
        		if(liste_nom[eleve_courant]==null)
        		{
        			label2.Text="Toutes les photos sont prises";
					button1.Enabled=false;
					finclasse=true;
        			return;
        		}
				EleveSuivant();
        	}
        }
        
        void Button1Click(object sender, EventArgs e)
        {
            Helper.SaveImageCaptureAuto(imgCapture.Image,Application.StartupPath+"\\"+comboBox1.SelectedItem.ToString()+"\\"+liste_nom[eleve_courant]+".jpg");
            EleveSuivant();
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
