/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 10/04/2010
 * Heure: 14:11
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;

using System.Diagnostics;
using VncSharp;

namespace Vnc_Manager
{
	public class LabelArray : System.Collections.CollectionBase
	{
		private readonly System.Windows.Forms.Form HostForm;

		public LabelArray(System.Windows.Forms.Form host)
		{
			HostForm = host;
		}

		public void Add(string poste, int posx, int posy)
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

			// Crée une nouvelle instance de la classe Button.
			System.Windows.Forms.Label aLabel = new System.Windows.Forms.Label();
			// Ajoute le bouton à la liste interne de la collection.
			this.List.Add(aLabel);
			// Ajoute le bouton à la collection de contrôles du formulaire 
			// référencé dans le champ HostForm.
			HostForm.Controls.Add(aLabel);
			aLabel.Location = new System.Drawing.Point(posx*5, posy*5+60);
			aLabel.Name = "Label " + this.Count.ToString();
			aLabel.Size = new System.Drawing.Size(70, 15);
			aLabel.TabIndex = this.Count;
			aLabel.Text = poste;
			aLabel.Click += new System.EventHandler(ClickHandler);
		}
		public System.Windows.Forms.Label this [int Index]
		{
			get
			{
				return (System.Windows.Forms.Label) this.List[Index];
			}
		}
		public void Remove()
		{
			while (this.Count > 0)
			{
				// Supprime le dernier bouton ajouté 
				// au tableau de la collection des contrôles 
				// du formulaire hôte. Remarquez l'utilisation 
				// de l'indexeur en accédant au tableau.
//Attention! le premier tableau contient aussi les menus au début,il y a un décalage.
				HostForm.Controls.Remove(this[this.Count -1]);
				this.List.RemoveAt(this.Count -1);
			}
		}
		public void ClickHandler(Object sender, System.EventArgs e)
		{
			Process myProc;
			myProc = Process.Start("vncviewer.exe", "-connect " + ((System.Windows.Forms.Label) sender).Text + " -password -viewonly");
			myProc.WaitForExit();
		}
	}

	public class PictureBoxArray : System.Collections.CollectionBase
	{
		private readonly System.Windows.Forms.Form HostForm;

		public PictureBoxArray(System.Windows.Forms.Form host)
		{
			HostForm = host;
		}

		public void Add(string poste, int posx, int posy)
		{
			VncSharp.RemoteDesktop rd = new VncSharp.RemoteDesktop();

			// Ajoute le bouton à la liste interne de la collection.
			this.List.Add(rd);
			// Ajoute le bouton à la collection de contrôles du formulaire 
			// référencé dans le champ HostForm.
			HostForm.Controls.Add(rd);
			// 
			// rd
			// 
			rd.AutoScroll = true;
			rd.AutoScrollMinSize = new System.Drawing.Size(150, 112);
			rd.Location = new System.Drawing.Point(posx*5, posy*5);
			rd.Name = "rd";
			rd.Size = new System.Drawing.Size(150, 112);
			rd.TabIndex = this.Count;
//			rd.GetPassword = passw;
			try
			{
				rd.Connect(poste,true,true);
			}
			catch (VncProtocolException vex)
			{
			}

				
/*				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
				// Crée une nouvelle instance de la classe Button.
				System.Windows.Forms.PictureBox aPicture = new System.Windows.Forms.PictureBox();
				// Ajoute le bouton à la liste interne de la collection.
				this.List.Add(aPicture);
				// Ajoute le bouton à la collection de contrôles du formulaire 
				// référencé dans le champ HostForm.
				HostForm.Controls.Add(aPicture);
				// Définit les propriétés initiales de l'objet button.
				aPicture.Image = ((System.Drawing.Image)(resources.GetObject("computer-icon")));
				aPicture.Location = new System.Drawing.Point(posx*5, posy*5);
				aPicture.Name = "Picture " + this.Count.ToString();;
				aPicture.Size = new System.Drawing.Size(70, 60);
				aPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
				aPicture.TabIndex = this.Count;
				aPicture.TabStop = false;
				aPicture.Text = poste;
				aPicture.Click += new System.EventHandler(ClickHandler);*/
		}
		public System.Windows.Forms.PictureBox this [int Index]
		{
			get
			{
				return (System.Windows.Forms.PictureBox) this.List[Index];
			}
		}
		public void Remove()
		{
			while (this.Count > 0)
			{
				// Supprime le dernier bouton ajouté 
				// au tableau de la collection des contrôles 
				// du formulaire hôte. Remarquez l'utilisation 
				// de l'indexeur en accédant au tableau.
//Attention! le premier tableau contient aussi les menus au début,il y a un décalage.
				HostForm.Controls.Remove(this[this.Count -1]);
				this.List.RemoveAt(this.Count -1);
			}
		}
		public void ClickHandler(Object sender, System.EventArgs e)
		{
			Process myProc;
			myProc = Process.Start("vncviewer.exe", "-connect " + ((System.Windows.Forms.PictureBox) sender).Text + " -password -viewonly");
			myProc.WaitForExit();
		}
	}
/*	public class MenuRDCArray : System.Collections.CollectionBase
	{
		private readonly System.Windows.Forms.Form HostForm;

		public MenuRDCArray(System.Windows.Forms.Form host)
		{
			HostForm = host;
		}

		public void Add(string poste)
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			// Crée une nouvelle instance de la classe Button.
			System.Windows.Forms.ToolStripMenuItem aMenuRDC = new System.Windows.Forms.ToolStripMenuItem();
			// Ajoute le bouton à la liste interne de la collection.
			//this.List.Add(aMenuRDC);
			//.RdcToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {aMenuRDC});
			// Ajoute le bouton à la collection de contrôles du formulaire 
			// référencé dans le champ HostForm.

//this.RdcToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.s1ToolStripMenuItem2});			

			HostForm.Controls.Add(aMenuRDC);
			// Définit les propriétés initiales de l'objet button.
			aMenuRDC.Name = poste;
			aMenuRDC.Size = new System.Drawing.Size(103, 22);
			aMenuRDC.Text = poste;
			aMenuRDC.Click += new System.EventHandler(MainForm. ClickHandler);
		}
		public System.Windows.Forms.PictureBox this [int Index]
		{
			get
			{
				return (System.Windows.Forms.PictureBox) this.List[Index];
			}
		}
		public void Remove()
		{
			while (this.Count > 0)
			{
				// Supprime le dernier bouton ajouté 
				// au tableau de la collection des contrôles 
				// du formulaire hôte. Remarquez l'utilisation 
				// de l'indexeur en accédant au tableau.
//Attention! le premier tableau contient aussi les menus au début,il y a un décalage.
				HostForm.Controls.Remove(this[this.Count -1]);
				this.List.RemoveAt(this.Count -1);
			}
		}
		public void ClickHandler(Object sender, System.EventArgs e)
		{
			//DessineSalle(((System.Windows.Forms.PictureBox) sender).Text);
		}
	}
*/}
