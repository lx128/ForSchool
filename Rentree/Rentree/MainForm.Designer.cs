/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 08/09/2010
 * Heure: 21:12
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
namespace RentreeGPAL
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.remiseÀZéroDesDeltasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fermerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aProposToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(296, 104);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 50);
			this.button1.TabIndex = 0;
			this.button1.Text = "Elèves";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(296, 48);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(100, 50);
			this.button2.TabIndex = 1;
			this.button2.Text = "Professeurs";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(12, 30);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(278, 227);
			this.richTextBox1.TabIndex = 2;
			this.richTextBox1.Text = "";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.fichierToolStripMenuItem,
									this.aProposToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(407, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fichierToolStripMenuItem
			// 
			this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.remiseÀZéroDesDeltasToolStripMenuItem,
									this.fermerToolStripMenuItem});
			this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
			this.fichierToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
			this.fichierToolStripMenuItem.Text = "Fichier";
			// 
			// remiseÀZéroDesDeltasToolStripMenuItem
			// 
			this.remiseÀZéroDesDeltasToolStripMenuItem.Name = "remiseÀZéroDesDeltasToolStripMenuItem";
			this.remiseÀZéroDesDeltasToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.remiseÀZéroDesDeltasToolStripMenuItem.Text = "Remise à zéro des Deltas";
			this.remiseÀZéroDesDeltasToolStripMenuItem.Click += new System.EventHandler(this.RemiseÀZéroDesDeltasToolStripMenuItemClick);
			// 
			// fermerToolStripMenuItem
			// 
			this.fermerToolStripMenuItem.Name = "fermerToolStripMenuItem";
			this.fermerToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.fermerToolStripMenuItem.Text = "Fermer";
			this.fermerToolStripMenuItem.Click += new System.EventHandler(this.FermerToolStripMenuItemClick);
			// 
			// aProposToolStripMenuItem
			// 
			this.aProposToolStripMenuItem.Name = "aProposToolStripMenuItem";
			this.aProposToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
			this.aProposToolStripMenuItem.Text = "A propos";
			this.aProposToolStripMenuItem.Click += new System.EventHandler(this.AProposToolStripMenuItemClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(296, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(99, 15);
			this.label1.TabIndex = 4;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(407, 266);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "Rentrée";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripMenuItem fermerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem remiseÀZéroDesDeltasToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aProposToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
	}
}
