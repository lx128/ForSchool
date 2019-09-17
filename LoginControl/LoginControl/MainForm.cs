/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 15/06/2010
 * Heure: 15:42
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Net;

namespace LoginControl
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			Init();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		private void Init()
		{
			byte[] buf = new byte[1500];
			string rec_buffer;
			System.Text.ASCIIEncoding  encoding=new System.Text.ASCIIEncoding();

			UdpClient listener = new UdpClient(1025);
			IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 1025);

			buf = listener.Receive(ref groupEP);
			rec_buffer = encoding.GetString(buf);
			richTextBox1.AppendText(rec_buffer);
		}
	}
}
