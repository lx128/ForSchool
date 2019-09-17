/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 05/03/2010
 * Heure: 12:00
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */

using System.Windows.Forms;

namespace Vnc_Manager
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
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.button2 = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.button5 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.agrandirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redémarrerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.rafraichirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rétrécirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.controleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.rétrécirToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.redémarrerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.contextMenuStrip.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(255, 1);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(220, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Sélectionner une salle";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(992, 1);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(23, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "?";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(1, 1);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 5;
			this.comboBox1.DropDownClosed += new System.EventHandler(this.ComboBox1DropdownClosed);
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(128, 1);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(121, 21);
			this.comboBox2.TabIndex = 5;
			this.comboBox2.DropDownClosed += new System.EventHandler(this.ComboBox2DropdownClosed);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(898, 1);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(90, 23);
			this.button2.TabIndex = 6;
			this.button2.Text = "Carte de la salle";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Visible = false;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(481, 1);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(505, 23);
			this.progressBar1.TabIndex = 7;
			this.progressBar1.Visible = false;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(817, 1);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 8;
			this.button3.Text = "Rafraichir";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Visible = false;
			this.button3.Click += new System.EventHandler(this.Button3Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(736, 1);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 9;
			this.button4.Text = "Noms";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Visible = false;
			this.button4.Click += new System.EventHandler(this.Button4Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(449, 229);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 23);
			this.button5.TabIndex = 10;
			this.button5.Text = "Démarrer";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.Button5Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(372, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(384, 122);
			this.label2.TabIndex = 11;
			this.label2.Text = "label2";
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(655, 1);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 23);
			this.button6.TabIndex = 12;
			this.button6.Text = "Contrôle";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Visible = false;
			this.button6.Click += new System.EventHandler(this.ContrôleToolStripMenuItemClick);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(574, 1);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(75, 23);
			this.button7.TabIndex = 13;
			this.button7.Text = "Photo";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Visible = false;
			this.button7.Click += new System.EventHandler(this.PrendreUnePhotoToolStripMenuItemClick);
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.agrandirToolStripMenuItem,
									this.redémarrerToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip1";
			this.contextMenuStrip.Size = new System.Drawing.Size(151, 48);
			// 
			// agrandirToolStripMenuItem
			// 
			this.agrandirToolStripMenuItem.Name = "agrandirToolStripMenuItem";
			this.agrandirToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.agrandirToolStripMenuItem.Text = "Voir en Grand";
			this.agrandirToolStripMenuItem.Click += new System.EventHandler(this.VoirEnGrandToolStripMenuItemClick);
			// 
			// redémarrerToolStripMenuItem
			// 
			this.redémarrerToolStripMenuItem.Name = "redémarrerToolStripMenuItem";
			this.redémarrerToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.redémarrerToolStripMenuItem.Text = "Redémarrer";
			this.redémarrerToolStripMenuItem.Click += new System.EventHandler(this.RedémarrerToolStripMenuItemClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.rafraichirToolStripMenuItem,
									this.rétrécirToolStripMenuItem,
									this.controleToolStripMenuItem1,
									this.rétrécirToolStripMenuItem1,
									this.redémarrerToolStripMenuItem1});
			this.contextMenuStrip1.Name = "contextMenuStrip2";
			this.contextMenuStrip1.Size = new System.Drawing.Size(176, 114);
			// 
			// rafraichirToolStripMenuItem
			// 
			this.rafraichirToolStripMenuItem.Name = "rafraichirToolStripMenuItem";
			this.rafraichirToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.rafraichirToolStripMenuItem.Text = "Rafraichir";
			this.rafraichirToolStripMenuItem.Click += new System.EventHandler(this.Button3Click);
			// 
			// rétrécirToolStripMenuItem
			// 
			this.rétrécirToolStripMenuItem.Name = "rétrécirToolStripMenuItem";
			this.rétrécirToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.rétrécirToolStripMenuItem.Text = "Prendre une photo";
			this.rétrécirToolStripMenuItem.Click += new System.EventHandler(this.PrendreUnePhotoToolStripMenuItemClick);
			// 
			// controleToolStripMenuItem1
			// 
			this.controleToolStripMenuItem1.Name = "controleToolStripMenuItem1";
			this.controleToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
			this.controleToolStripMenuItem1.Text = "Contrôle";
			this.controleToolStripMenuItem1.Click += new System.EventHandler(this.ContrôleToolStripMenuItemClick);
			// 
			// rétrécirToolStripMenuItem1
			// 
			this.rétrécirToolStripMenuItem1.Name = "rétrécirToolStripMenuItem1";
			this.rétrécirToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
			this.rétrécirToolStripMenuItem1.Text = "Rétrécir";
			this.rétrécirToolStripMenuItem1.Click += new System.EventHandler(this.Button4Click);
			// 
			// redémarrerToolStripMenuItem1
			// 
			this.redémarrerToolStripMenuItem1.Name = "redémarrerToolStripMenuItem1";
			this.redémarrerToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
			this.redémarrerToolStripMenuItem1.Text = "Redémarrer";
			this.redémarrerToolStripMenuItem1.Click += new System.EventHandler(this.RedémarrerToolStripMenuItemClick);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Red;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(1, 1);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(121, 23);
			this.label3.TabIndex = 14;
			this.label3.Text = "Mon poste";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Green;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(128, 1);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(121, 23);
			this.label4.TabIndex = 15;
			this.label4.Text = "Poste élève";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.ClientSize = new System.Drawing.Size(1016, 734);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.comboBox2);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Name = "MainForm";
			this.Text = "Vnc Manager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.contextMenuStrip.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ToolStripMenuItem rafraichirToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem redémarrerToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem rétrécirToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem controleToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem rétrécirToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem redémarrerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem agrandirToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
	}
}
