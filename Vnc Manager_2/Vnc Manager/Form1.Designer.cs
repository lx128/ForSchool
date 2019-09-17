/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 26/01/2011
 * Heure: 09:05
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
namespace Vnc_Manager
{
	partial class Form1
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
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.rafraichirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nomDesPostesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(250, 238);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(300, 50);
			this.label2.TabIndex = 8;
			this.label2.Text = "Déconnexion en cours";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label2.Visible = false;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(250, 238);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(300, 50);
			this.label1.TabIndex = 7;
			this.label1.Text = "Connexion en cours";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.Visible = false;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(250, 291);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(300, 50);
			this.progressBar1.TabIndex = 6;
			this.progressBar1.Visible = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.rafraichirToolStripMenuItem,
									this.nomDesPostesToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(792, 24);
			this.menuStrip1.TabIndex = 9;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// rechargerToolStripMenuItem
			// 
			this.rafraichirToolStripMenuItem.Name = "rafraichirToolStripMenuItem";
			this.rafraichirToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
			this.rafraichirToolStripMenuItem.Text = "Rafraichir";
			this.rafraichirToolStripMenuItem.Click += new System.EventHandler(this.RafraichirToolStripMenuItemClick);
			// 
			// nomDesPostesToolStripMenuItem
			// 
			this.nomDesPostesToolStripMenuItem.Name = "nomDesPostesToolStripMenuItem";
			this.nomDesPostesToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
			this.nomDesPostesToolStripMenuItem.Text = "Nom des postes";
			this.nomDesPostesToolStripMenuItem.Click += new System.EventHandler(this.NomDesPostesToolStripMenuItemClick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripMenuItem nomDesPostesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rafraichirToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}
