/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 26/01/2011
 * Heure: 09:05
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using VncSharp;
using System.Threading;
using System.Net.NetworkInformation;

namespace Vnc_Manager
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class Form1 : Form
	{
		private const byte NBELEMENT = 19;
		private byte nb_poste=0;
		private bool etat_des_postes=true;
		public bool actif=false;
		private string[] liste_poste = new string[NBELEMENT];
		private RemoteDesktop[] MyRDArray= new RemoteDesktop[NBELEMENT];
		private Label[] LabelArray= new Label[NBELEMENT];

		public Form1(string [] poste)
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
		~Form1()
		{
			Form1_FormClosing(null,null);
		}
		private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
		{
			label2.Visible=true;
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
			label2.Visible=false;
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
        private string passw()
        {
        	return "";
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
}
