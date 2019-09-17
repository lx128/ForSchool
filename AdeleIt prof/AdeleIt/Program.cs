/*
 * Crée par SharpDevelop.
 * Utilisateur: User
 * Date: 28/01/2012
 * Heure: 21:56
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;
using System.IO;
using System.DirectoryServices;
using Microsoft.Win32;

namespace AdeleIt
{
	class Program
	{
		public static void Main(string[] args)
		{
			string login="prof.test", mat="FRANCAIS", str, prenom, nom, login2;
			int comp;

			login=args[0];
			mat=args[1];


			comp=login.IndexOf(".");
			prenom=login.Substring(0,comp);
			nom=login.Substring(comp+1,login.Length-comp-1);
			login2=nom+"."+prenom;
			comp=ReadValue();
			Console.WriteLine(comp);

			//Déplacement répartoire
			Directory.Move("c:\\Users\\profs\\"+login,"c:\\Adele2k3\\profs\\"+login2);

			//Mise à jour utilisateur
			DirectoryEntry Ldap = new DirectoryEntry("LDAP://DC1");

			DirectorySearcher searcher = new DirectorySearcher(Ldap);
			searcher.Filter = "(SAMAccountName="+login+")";
			// Pas de boucle foreach car on ne cherche qu'un user
			SearchResult result = searcher.FindOne();
			// On récupère l'objet trouvé lors de la recherche
			DirectoryEntry DirEntry = result.GetDirectoryEntry();
			// On modifie la propriété description de l'utilisateur TEST
			str=DirEntry.Properties["cn"].Value+"    "+comp.ToString();
//			DirEntry.Properties["cn"].Value = str;
			DirEntry.Properties["description"].Value = mat;
			DirEntry.Properties["employeeID"].Value = "Passw";
			DirEntry.Properties["employeeType"].Value = comp.ToString();
			DirEntry.Properties["info"].Value = "01/01/1990";
			DirEntry.Properties["facsimileTelephoneNumber"].Value = "S"+(comp+1).ToString();
			DirEntry.Properties["homeDrive"].Value = "U:";
			DirEntry.Properties["homeDirectory"].Value = "\\\\DC1\\profs$\\"+login2;
			DirEntry.Properties["homePhone"].Value = "S"+(comp+1).ToString();
			DirEntry.Properties["mail"].Value = "@xxxx.local";
//			DirEntry.Properties["name"].Value = "U:";
			DirEntry.Properties["pager"].Value = comp.ToString();
			DirEntry.Properties["profilePath"].Value = "\\\\DC1\\profils$\\profs";
			DirEntry.Properties["scriptPath"].Value = "prof.vbs";
			DirEntry.Properties["telephoneNumber"].Value = "S"+(comp+1).ToString();
			DirEntry.Properties["mobile"].Value = "0";
			DirEntry.Properties["ipPhone"].Value = "1";
			DirEntry.Properties["wWWHomePage"].Value = mat;
			DirEntry.Properties["userPrincipalName"].Value = login+"@xxxx.local";
//			Console.WriteLine(DirEntry.Properties["name"].Value);

			//changement des droits
			DirectoryEntry TestGroup = new DirectoryEntry("LDAP://DC1/CN=g_profs,CN=Users,DC=xxxx");//,DC=local");
			TestGroup.Properties["member"].Add(DirEntry.Properties["distinguishedName"].Value);
			TestGroup.CommitChanges();
			TestGroup = new DirectoryEntry("LDAP://DC1/CN=matiere_"+mat+",OU=groupes,DC=xxxx");//,DC=local");
			TestGroup.Properties["member"].Add(DirEntry.Properties["distinguishedName"].Value);
			TestGroup.CommitChanges();
			TestGroup.Close();

			// On envoie les changements à Active Directory
			DirEntry.CommitChanges();

			//déplacement dans AD
			DirEntry.MoveTo(new DirectoryEntry("LDAP://DC1/OU=profs,DC=xxxx"));//,DC=local");
			DirEntry.Close();

			//Renommage
			searcher = new DirectorySearcher(Ldap);
			searcher.Filter = "(SAMAccountName="+login+")";
			result = searcher.FindOne();
			DirEntry = result.GetDirectoryEntry();
			DirectoryEntry child = new DirectoryEntry(result.Path);
			child.Rename("CN=" + nom+" "+prenom);

			Console.WriteLine(Ldap.Name);
			/* Get the underlying directory entry from the principal
			System.DirectoryServices.DirectoryEntry UnderlyingDirectoryObject =
			     PrincipalInstance.GetUnderlyingObject() as System.DirectoryServices.DirectoryEntry;
			
			// Read the content of the 'notes' property (It's actually called info in the AD schema)
			string NotesPropertyContent = UnderlyingDirectoryObject.Properties["info"].Value;
			
			// Set the content of the 'notes' field (It's actually called info in the AD schema)
			UnderlyingDirectoryObject.Properties["info"].Value = "Some Text";
			
			// Commit changes to the directory entry
			UserDirectoryEntry.CommitChanges();*/

			WriteValue(comp+2);
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		static int ReadValue()
		{
			int i=0;
			RegistryKey Nkey = Registry.LocalMachine;
			try
			{
				RegistryKey valKey = Nkey.OpenSubKey("Software\\Adele2k3\\", true);
				if(valKey == null)
				{
					Console.WriteLine("Erreur lecture de la clé NumINE");
					return 0;
				}
				else
				{
					i = (int)valKey.GetValue("NumINE");
					valKey.Close();
				}
			}
			catch(Exception er)
			{
				Console.WriteLine("Erreur lecture de la clé NumINE");
			}
			finally
			{
				Nkey.Close();
			}
			return i;
		}
		
		//Fonction qui écrit le mot de passe dans la base de registre windows
		static void WriteValue(int i)
		{
			RegistryKey Nkey = Registry.LocalMachine;

			RegistryKey valKey = Nkey.OpenSubKey("Software\\Adele2k3", true);
			valKey.SetValue("NumINE", i);
			Nkey.Close();
		}
	}
}