using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace WinFormCharpWebCam
{
    //Design by Pongsakorn Poosankam
    class Helper
    {

        public static void SaveImageCapture(System.Drawing.Image image)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "Image";// Default file name
            s.DefaultExt = ".Jpg";// Default file extension
            s.Filter = "Image (.jpg)|*.jpg"; // Filter files by extension

            // Show save file dialog box
            // Process save file dialog box results
            if (s.ShowDialog()==DialogResult.OK)
            {
                // Save Image
                string filename = s.FileName;
				if(image!=null)
				{
	                FileStream fstream = new FileStream(filename, FileMode.Create);
	                image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
	                fstream.Close();
	                //Image i = Image.FromFile(filename);
	                //i.RotateFlip(RotateFlipType.Rotate90FlipNone);
	                //i.Save(filename);
				}
            }
        }
        public static void SaveImageCaptureAuto(System.Drawing.Image image, string filename)
        {
			string path;
			path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase );
			path=new Uri(path).LocalPath;
			Directory.SetCurrentDirectory(@path);

			if(image!=null)
			{
	            FileStream fstream = new FileStream(filename, FileMode.Create);
	            image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
	            fstream.Close();
                //Image i = Image.FromFile(filename);
                //i.RotateFlip(RotateFlipType.Rotate90FlipNone);
                //i.Save(filename);
			}
        }
    }
}
