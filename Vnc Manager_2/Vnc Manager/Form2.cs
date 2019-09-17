/*
 * Created by SharpDevelop.
 * User: User
 * Date: 10/10/2011
 * Time: 13:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vnc_Manager
{
	/// <summary>
	/// Description of Form2.
	/// </summary>
	public partial class Form2 : Form
	{
		public Form2()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		void Timer1Tick(object sender, EventArgs e)
		{
			if (progressBar1.Value<=0)
			{
				progressBar1.Value=100;
			}
			progressBar1.Value=progressBar1.Value-5;
		}
	}
}
