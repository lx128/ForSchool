/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 15/06/2010
 * Heure: 16:43
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;

using System.Net;
using System.Net.Sockets;

namespace LoginControl_Client
{
	class Program
	{
		public static void Main(string[] args)
		{
			byte[] send_buffer = new byte[10];
			Console.WriteLine("Hello World!");
			
			// TODO: Implement Functionality Here
			Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			IPAddress send_to_address = IPAddress.Parse("10.130.75.6");
			IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 1025);

			System.Text.ASCIIEncoding  encoding=new System.Text.ASCIIEncoding();
    		send_buffer = encoding.GetBytes("Salut");

			sending_socket.SendTo(send_buffer, sending_end_point);

			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}