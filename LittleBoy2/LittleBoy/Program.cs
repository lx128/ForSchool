/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 22/09/2010
 * Heure: 09:35
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.Windows.Forms;

//using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
//using System.Text;
//using System.Net;

namespace LittleBoy
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
			LittleBoy run = new LittleBoy();
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new MainForm());
		}
	}
	class LittleBoy
	{
		NamedPipeServerStream pipeServer = new NamedPipeServerStream("MyPipe", PipeDirection.Out);
		public LittleBoy()
		{
			pipeServer.WaitForConnection();
			StreamWriter swa = new StreamWriter(pipeServer);
			try
			{
			// Read user input and send that to the client process.
				{
					swa.AutoFlush = true;
					swa.WriteLine("Ca marche!!!!!!!!!!!!!!!!");
				}
			}
			// Catch the IOException that is raised if the pipe is
			// broken or disconnected.
			catch (IOException f)
			{
				MessageBox.Show(f.Message);
			}
			swa.Close();
			pipeServer.Close();
		}
	}
}
