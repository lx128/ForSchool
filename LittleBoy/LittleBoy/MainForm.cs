/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 22/09/2010
 * Heure: 09:35
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Net;

namespace LittleBoy
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		static System.Timers.Timer tm = new System.Timers.Timer(5000);//Anti counter strike
		static System.Timers.Timer tm2 = new System.Timers.Timer(5000);//GRR
		static System.Timers.Timer tm3 = new System.Timers.Timer(1000);//compteur pour extiction forcé.
		short compteur_echec, compteur_succes;//pour controle de connexion sans cable réseau
		string nom_machine, nom_salle;

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			//InitializeComponent();

			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			NamedPipeServerStream pipeServer = new NamedPipeServerStream("MyPipe", PipeDirection.Out);

			pipeServer.WaitForConnection();
			try
			{
			// Read user input and send that to the client process.
				StreamWriter sw = new StreamWriter(pipeServer);
				{
					sw.AutoFlush = true;
					sw.WriteLine("Ca marche!!!!!!!!!!!!!!!!");
				}
			}
			// Catch the IOException that is raised if the pipe is
			// broken or disconnected.
			catch (IOException e)
			{
				MessageBox.Show("ERROR: "+ e.Message);
			}

			compteur_echec=0;
			compteur_succes=0;
			if (File.Exists("c:/disable")==true)
			{
				return;
			}

			if (!File.Exists(Environment.GetEnvironmentVariable("userprofile")+"\\Bureau\\Connexion au serveur pronote.lnk"))
			{
					tm2.Elapsed+= new System.Timers.ElapsedEventHandler(AppelGRR);
					tm2.Enabled=true;
					tm2.Start();
			}

			tm.Elapsed+= new System.Timers.ElapsedEventHandler(BoucleTest);
			tm.Enabled=true;
			tm.Start();

			ListeNoire();

			while(true)
			{
				System.Threading.Thread.Sleep(10000);
			}
		}
		private void BoucleTest(object source, System.Timers.ElapsedEventArgs e)
		{
			string[] liste= new string[2];
			int comp, comp2;
			Process[] prc;

			//Programme interdit
			liste[0]="DATOS";//CS
			liste[1]="ZEROX";//CS
//			Console.Write(liste.Length);
			for(comp=0;liste.Length!=comp;comp++)
			{
				prc = Process.GetProcessesByName(liste[comp]);
				for(comp2=0;prc.Length!=comp2;comp2++)
				{
					prc[comp2].Kill();
				}
			}
			if (nom_salle=="C112"|nom_salle=="CDI"|nom_salle=="C119")
			{
				//Empeche l'utilisation de l'ordi sans cable réseau
				if (File.Exists(@"\\Pdc\GSapps\bonjour.exe"))
				{
					compteur_succes++;
				}
				else
				{
		//				MessageBox.Show("aaaaaaaaaa");
					compteur_echec++;
				}
				if(compteur_succes==6)
				{
					compteur_succes=0;
					compteur_echec=0;
				}
				if(compteur_echec==5)
				{
					tm.Dispose();
					tm3.Elapsed+= new System.Timers.ElapsedEventHandler(Ferme);
					tm3.Enabled=true;
					tm3.Start();
				}
			}
		}
		private void AppelGRR(object source, System.Timers.ElapsedEventArgs e)
		{
			int pos, lastpos=0, comp, marge;
			bool flag=false;
			string s, word, salles =
				"A102-A111-A113-A114-A118-A119-A124-A207-"+
				"B100-B108-B204-B207-B208-"+
				"C102-C105-C109-C112-C115-C120-C121-C123-C206-C222";

			//Nom salle
			nom_machine = Environment.MachineName.ToString();
			pos = nom_machine.IndexOf("-");
			nom_salle=nom_machine.Substring(0,pos);
			nom_salle = nom_salle.ToUpper();
//nom_salle="C123";
			//Salles Exclues du système de contrôle
			if (salles.Contains(nom_salle)==false)
			{
//				Console.WriteLine("Salles exclues");
//				Console.ReadKey(true);
				tm2.Dispose();
				return;
			}

			Uri uri = new Uri("http://xxxxxxx/grr/grr_control.php");
			WebRequest req = WebRequest.Create(uri);
			WebResponse resp=null;
			try
			{
				resp = req.GetResponse();
			}
			catch(WebException exp)
			{
				//MessageBox.Show(exp.Message);
				return;
			}
			Stream stream = resp.GetResponseStream();
			StreamReader sr = new StreamReader(stream);
			s = sr.ReadLine();


			while (s!=null&flag==false)
			{
				pos = s.IndexOf(";",lastpos);
				word= s.Substring(lastpos,(pos-lastpos));
				//test nom salle
				if (word.ToUpper()==nom_salle)
				{
					lastpos=pos+1;
					pos = s.IndexOf(";",lastpos);
					word= s.Substring(lastpos,(pos-lastpos));


					//Application d'une marge d'erreur de 5mn le matin et 15mn l'après midi
					if (DateTime.Now.Hour<13)
					{
						marge=300;//matin
					}
					else
					{
						marge=900;//AM
					}
					//test heure début
					comp = int.Parse(word)-marge;
					if (comp<=(int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)
					{
						lastpos=pos+1;
						pos = s.IndexOf(";",lastpos);
						word= s.Substring(lastpos,(pos-lastpos));
						//test heure fin
						comp = int.Parse(word);
						if (comp>=(int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)
						{
							//Atribution du flag
							flag=true;
						}
					}
				}

				s = sr.ReadLine();
				pos=0;
				lastpos=0;
			}
			//Pas de droit donc extinction
			tm2.Dispose();
			if (flag==false)
			{
				tm3.Elapsed+= new System.Timers.ElapsedEventHandler(Ferme);
				tm3.Enabled=true;
				tm3.Start();
				MessageBox.Show("Cette salle n'a pas été réservé dans le logiciel de réservation\n"+
				               "Veuillez contacter le chef de travaux\n\n Votre poste va se fermer.");
				return;
			}
			//MessageBox.Show("Poursuite démarrage Droit Valide");
			return;
		}
		private static void Ferme(object source, System.Timers.ElapsedEventArgs e)
		{
			short comp;
			Process[] prc = Process.GetProcesses();

			for(comp=0;comp<2;comp++)
			{
				prc = Process.GetProcessesByName("explorer");
//				prc[0].Kill();
				foreach(Process p in prc)
				{
//					MessageBox.Show(p.ProcessName);
					if (p.ProcessName!="svchost"
					   & p.ProcessName!="srvchost"
					   & p.ProcessName!="spoolsv"
					   & p.ProcessName!="avp"
					  )
					{
						//MessageBox.Show("Kill");
//						p.Kill();
					}
				}
			}
			MessageBox.Show("Eteindre le poste");
//			Process.Start("c:/windows/system32/shutdown","-l -f");
		}
		private static void ListeNoire()//autorisé sauf dans certaines salles
		{
			FonctionCLR myfunc=new FonctionCLR();
			FonctionFichierBinaireCLR binfile=new FonctionFichierBinaireCLR("Liste.csv");
			string line,element,login,salle,nom_machine;
			byte comp;
			int pos;
			bool flag=false;

			login=Environment.UserName;
			nom_machine = Environment.MachineName.ToString();
			pos = nom_machine.IndexOf("-");
			salle=nom_machine.Substring(0,pos);

//login="";
//salle="C123";
//MessageBox.Show("Liste");
			while(!binfile.EndOfStream)
			{
				line=binfile.LireLigne();
				myfunc.element(line,0,out element,';');
				if(element.ToUpper()==login.ToUpper())
				{
//if(element==login){MessageBox.Show(login);}
					for(comp=1;element!="";comp++)
					{
//MessageBox.Show(element);
						myfunc.element(line,comp,out element,';');
						if(element.ToUpper()==salle.ToUpper())
						{
							//Interdit
//MessageBox.Show("ok");
							flag=true;
						}
					}
				}
			}
			if(flag)
			{
				if(!tm3.Enabled)
				{
					tm3.Elapsed+= new System.Timers.ElapsedEventHandler(Ferme);
					tm3.Enabled=true;
					tm3.Start();
				}
				MessageBox.Show("Vous n'êtes pas autorisé à vous connecter dans cette salle.\n"+
				               "Veuillez contacter le chef de travaux\n\n Votre poste va se fermer.");
			}
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
//		BinaryWriter fw;

		public FonctionFichierBinaireCLR(string str)
		{
			fs = new FileStream(str,FileMode.Open,FileAccess.Read);
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
