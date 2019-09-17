/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 29/11/2010
 * Heure: 12:33
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using System.IO;
using System.Text;

namespace LogSearch
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		string[] correspondance=new string[64];
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			string ligne,element;
			byte comp=0;
			FonctionCLR Fonction= new FonctionCLR();
			FonctionFichierBinaireCLR fichier = new FonctionFichierBinaireCLR("Classe.csv");
	
			comboBox1.Items.Add("");
			while(fichier.EndOfStream!=true)
			{
				ligne = fichier.LireLigne();
				Fonction.element(ligne,0,out element,'	');
				comboBox1.Items.Add(element);
				Fonction.element(ligne,1,out correspondance[comp],'	');
				comp++;
			}
		}
		private void Recherche(string salle,string login, string nom, string date)
		{
			string ligne,element="",classe="",resultat="";
			FonctionCLR Fonction= new FonctionCLR();
			byte res=0, nb=0;

			button1.Text="Patientez SVP";
			button1.Enabled=false;

			resultat="";
			salle=salle.ToUpper();
			nom=nom.ToUpper();
			login=login.ToUpper();
//MessageBox.Show(comboBox1.SelectedIndex.ToString());
			if(comboBox1.SelectedIndex!=0 & comboBox1.SelectedIndex!=-1)
			{
				classe = correspondance[comboBox1.SelectedIndex-1];
//MessageBox.Show(classe);
				res++;
			}
			if(salle!="")
			{
				res++;
			}
			if(login!="")
			{
				res++;
			}
			if(nom!="")
			{
				res++;
			}
			if(date!="")
			{
				res++;
			}

			DirectoryInfo di = new DirectoryInfo(".");
			FileInfo[] rgFiles = di.GetFiles("*.txt");
			foreach(FileInfo fi in rgFiles)
			{
				FonctionFichierBinaireCLR fichier = new FonctionFichierBinaireCLR(fi.Name);

				while(fichier.EndOfStream!=true)
				{
					ligne = fichier.LireLigne();
					nb=0;
					if (salle!="")
					{
						Fonction.element(ligne,0,out element,'-');
						if (element==salle)
						{
							nb++;
						}
					}
					if (comboBox1.SelectedIndex!=0 & comboBox1.SelectedIndex!=-1)
					{
						Fonction.element(ligne,1,out element,',');
						if (element==classe)
						{
							nb++;
						}
					}
					if (login!="")
					{
						Fonction.element(ligne,2,out element,',');
						if (element==login)
						{
							nb++;
						}
					}
					if (nom!="")
					{
						Fonction.element(ligne,3,out element,',');
						if (element.Contains(nom)==true)
						{
							nb++;
						}
					}
					if (date!="")
					{
						Fonction.element(ligne,4,out element,',');
						if (element.Contains(date)==true)
						{
							nb++;
						}
					}
					if(nb==res)
					{
						resultat+=ligne;
						resultat+="\n";
					}
				}
				fichier.Ferme();
			}
			button1.Enabled=true;
			button1.Text="Recherche";
			richTextBox1.Text=resultat;
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
