/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 25/01/2011
 * Heure: 16:35
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;

namespace BigBrother
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			//BigBrother run = new BigBrother();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new BigBrother());
			//Application.Run(new MainForm());
			//while(true)
			{
			//	System.Threading.Thread.Sleep(60000);
			}
		}
	}

	class BigBrother:Form
	{
		const string Domaine ="XXXX";
		static System.Timers.Timer tm = new System.Timers.Timer(5000);//Boucle principale
		static System.Timers.Timer tm2 = new System.Timers.Timer(5000);//GRR, Anti counter strike et autre
		static System.Timers.Timer tm3 = new System.Timers.Timer(5000);//compteur pour extiction forcé.
		short compteur_echec=0, compteur_succes=0;//pour controle de connexion sans cable réseau
		string nom_poste, nom_salle, nom_user, nom_user_long, message,
		liste_tache, salles =
				"A102-A111-A113-A114-A118-A119-A124-A207-"+
				"B100-B108-B204-B207-B208-"+
				"C102-C105-C109-C112-C115-C120-C121-C123-C206-C222";
		bool test_grr=false, test_grr_fait=false, changement=false, FlagMessage=false;
		System.Threading.Thread newThread, newThread2, newThread3, ThreadPipe;
		NamedPipeClientStream pipeClient;
		StreamReader sr;
		StreamWriter sw;
		public BigBrother()
		{
//			Process[] prc = Process.GetProcesses();
			int comp;
//			NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "MyPipe", PipeDirection.In);
//			string temp;

			if (File.Exists("c:/disable")==true)
			{
				return;
			}

			//Nom salle et poste
			nom_poste = Environment.MachineName.ToString();
			comp = nom_poste.IndexOf("-");
			nom_salle=nom_poste.Substring(0,comp);
			nom_salle = nom_salle.ToUpper();
//nom_salle="C123";

			//Salles Incluses dans le test GRR
			if (salles.Contains(nom_salle)==true)
			{
//				Console.WriteLine("Salles concernées");
				test_grr=true;
			}
			else
			{
				test_grr=false;
			}
			tm.Elapsed+= new System.Timers.ElapsedEventHandler(BouclePrincipale);
			tm.Enabled=true;

			tm2.Elapsed+= new System.Timers.ElapsedEventHandler(BoucleUser);
			tm2.Enabled=true;

			tm3.Elapsed+= new System.Timers.ElapsedEventHandler(Ferme);
			tm3.Enabled=false;

			//ThreadPipe = new System.Threading.Thread(this.ThreadPipeF);
		}
		void ThreadPipeF()
		{
			pipeClient = new NamedPipeClientStream(".", "MyPipe", PipeDirection.In);
			pipeClient.Connect();
			sr = new StreamReader(pipeClient);
			while(true)
			{
				if ((message = sr.ReadLine()) != null)
				{
					MessageBox.Show(message.ToString(),"Alerte !",MessageBoxButtons.OK,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification);
					pipeClient.Dispose();
					pipeClient = new NamedPipeClientStream(".", "MyPipe", PipeDirection.In);
					pipeClient.Connect();
					sr = new StreamReader(pipeClient);
				}
			}
		}
		void BouclePrincipale(object source, System.Timers.ElapsedEventArgs e)
		{
			string[] lines,words, element, element2;
			bool explorer;

			//recherche du nom de l'utilisateur
			explorer=false;
			ListeTache();
			lines=liste_tache.Split('\n');
			foreach(string line in lines)
			{
				words=line.Split(',');
				if (words.Length>1)
				{
					if ((words[0]=="\"explorer.exe\""))
					{
						explorer=true;
						if(changement==false)
						{
							//MessageBox.Show(words[0]);
							element=words[6].Split('\"');
							nom_user_long=element[1];
							element2=element[1].Split('\\');
							nom_user=element2[1];
							//System.Security.Principal.
							changement=true;
							//ListeNoire();
							tm2.Enabled=true;
						}
					}
				}
			}
			if (explorer)
			{
				// Display the read text to the console
				while (FlagMessage)
				{
					MessageBox.Show(message,"Alerte !",MessageBoxButtons.OK,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification);
					FlagMessage=false;
				}
				if (!ThreadPipe.IsAlive)
				{
					ThreadPipe = new System.Threading.Thread(this.ThreadPipeF);
					ThreadPipe.Start();
				}
			}
			else
			{
				tm2.Enabled=false;
				nom_user=nom_user_long=null;
				changement=false;
			}


			//Fonctions.element(liste_tache,0,out line,'\n');
/*			if (!File.Exists(""+"\\Bureau\\Connexion au serveur pronote.lnk"))
			{
				MessageBox.Show(Environment.GetEnvironmentVariable("userprofile"));
				test_grr=true;
			}*/
		}
		void ListeTache()
		{
		    ProcessStartInfo info = new ProcessStartInfo( );
		    info.FileName = "tasklist";
		    info.Arguments = "/V /FO CSV";
		    info.UseShellExecute = false;
		    info.RedirectStandardOutput = true;
		    info.CreateNoWindow = true;

		    try
		    {
		        Process p = Process.Start( info );
		        p.Start( );
		        liste_tache = p.StandardOutput.ReadToEnd( );
		        p.WaitForExit( /* 10000 */ );
		        p.Close( );
		    }
		    catch ( Exception ex )
		    {
//		        MessageBox.Show( ex.ToString( ) );
		    }
		}
		bool TestGRR()//retourne si le test est fait ou non
		{
			int pos, lastpos=0, comp, marge;
			bool flag=false;
			string s, word;
			Uri uri = new Uri("http://xxxxxxxx/grr/grr_control.php");
			WebRequest req = WebRequest.Create(uri);
			WebResponse resp=null;

			try
			{
				resp = req.GetResponse();
			}
			catch(WebException exp)
			{
				//MessageBox.Show(exp.Message);
				return false;
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
			tm2.Enabled=false;
			if (flag==false)
			{
				tm3.Enabled=true;
				newThread2 = new System.Threading.Thread(this.MessageGRR);
				newThread2.Start();
			}
			//MessageBox.Show("Poursuite démarrage Droit Valide");
			return true;
		}
		void MessageGRR()
		{
				MessageBox.Show("Cette salle n'a pas été réservé dans le logiciel de réservation\n"+
				               "Veuillez contacter le chef de travaux\n\n Votre poste va se fermer.");
		}
		void ProgInterdit()
		{
			int comp, comp2;
			string[] liste= new string[2];
			Process[] prc;

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
		}
		private void CableDebranché()
		{
			Ping EnvoiPing = new Ping();
			PingReply rep;

			if (nom_salle=="C112"|nom_salle=="CDI"|nom_salle=="C119")
			{
				//Empeche l'utilisation de l'ordi sans cable réseau
				//Test poste en ligne
				rep = EnvoiPing.Send("10.130.75.1");
				if (rep.Status != IPStatus.Success)
				//if(false)
				{
					compteur_succes++;
				}
				else
				{
					compteur_echec++;
					newThread = new System.Threading.Thread(this.MessageCable);
					newThread.Start();
				}
				if(compteur_succes==3)
				{
					compteur_succes=0;
					compteur_echec=0;
				}
				if(compteur_echec==5)
				{
//MessageBox.Show(nom_user);
					sw = new StreamWriter("Log.txt",true);
					sw.WriteLine(nom_user+" "+DateTime.Now+"\n");
					sw.Close();
					tm2.Enabled=false;
					tm3.Enabled=true;
				}
			}
		}
		void MessageCable()
		{
			MessageBox.Show("Attention! Cable réseau débranché! L'ordinateur va s'éteindre si le cable n'est pas rebranché immédiatement.","Alerte",MessageBoxButtons.OK,MessageBoxIcon.Stop,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly);
		}
		void BoucleUser(object source, System.Timers.ElapsedEventArgs e)
		{
/*			if((test_grr_fait==false) & (test_grr==true))
			{
				test_grr_fait=TestGRR();
			}*/
			ProgInterdit();
			CableDebranché();
		}
		void Ferme(object source, System.Timers.ElapsedEventArgs e)
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
		void ListeNoire()//autorisé sauf dans certaines salles
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
				tm3.Enabled=true;
				newThread3 = new System.Threading.Thread(this.MessageListeNoire);
				newThread3.Start();
			}
		}
		private void MessageListeNoire()
		{
				MessageBox.Show("Vous n'êtes pas autorisé à vous connecter dans cette salle.\n"+
				               "Veuillez contacter le chef de travaux\n\n Votre poste va se fermer.");
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
