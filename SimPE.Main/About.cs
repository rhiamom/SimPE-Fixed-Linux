/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;


namespace SimPe
{
	/// <summary>
	/// Zusammenfassung f�r About.
	/// </summary>
	public class About : SimPe.Windows.Forms.HelpForm
    {
		private Avalonia.Controls.TextBox rtb;
		private Avalonia.Controls.Button button1;
        private Avalonia.Controls.Button button2;
        private object wb;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public About() 
            :this(false)
		{
        }
		public About(bool html)
		{
			//
			// Erforderlich f�r die Windows Form-Designerunterst�tzung
			//
			InitializeComponent();
            // wb not available in Avalonia port
			rtb.IsVisible = !html;
		}

        void wb_Navigated(object sender, EventArgs e)
        {
            
        }

        void wb_Navigating(object sender, EventArgs e) { }

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.rtb = new Avalonia.Controls.TextBox { IsReadOnly = true };
			this.button1 = new Avalonia.Controls.Button();
			this.button2 = new Avalonia.Controls.Button();
			this.button2.Click += (s, e) => button2_Click(s, null);
		}
		#endregion

		void LoadResource(string flname)
		{
            rtb.IsVisible = true;
			System.Diagnostics.FileVersionInfo v = Helper.SimPeVersion;
			System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream("SimPe."+flname+"-"+System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName+".rtf");
			if (s==null) s = this.GetType().Assembly.GetManifestResourceStream("SimPe."+flname+"-en.rtf");
			if (s!=null) 
			{
				System.IO.StreamReader sr = new System.IO.StreamReader(s);
				string vtext = Helper.VersionToString(v); //v.FileMajorPart +"."+v.FileMinorPart;
				if (Helper.QARelease) vtext = "QA " + vtext;
				if (Helper.DebugMode) vtext += " [debug]";
				rtb.Text = sr.ReadToEnd().Replace("\\{Version\\}", vtext);
			} 
			else 
			{
				rtb.Text = "Error: Unknown Resource "+flname+".";
			}
		}

		/// <summary>
		/// Display the About Screen
		/// </summary>
		public static void ShowAbout()
		{
			About f = new About();
			f.Title = SimPe.Localization.GetString("About");

			f.LoadResource("about");
            SimPe.Splash.Screen.Stop();
			f.ShowDialog();
		}

		/// <summary>
		/// Display the Welcome Screen
		/// </summary>
		public static void ShowWelcome()
		{
			About f = new About();
			f.Title = SimPe.Localization.GetString("Welcome");
            f.LoadResource("welcome");
            SimPe.Splash.Screen.Stop();

            // TODO: "Don't show again" checkbox not ported to Avalonia
			f.ShowDialog();

            // Helper.XmlRegistry.ShowWelcomeOnStartup = ...; // not ported
		}

        //static System.Threading.Thread uthread;

		/// <summary>
		/// Search for Updates in an async Thread
		/// </summary>
		
        private static string GetHtmlBase()
        {
            System.IO.Stream s = typeof(About).Assembly.GetManifestResourceStream("SimPe.simpe.html");
            string html = "{CONTENT}";
            if (s != null)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(s);
                html = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return html;
        }

		static string TutorialTempFile
		{
			get 
			{
				return System.IO.Path.Combine(Helper.SimPeDataPath, "tutorialtemp.rtf");
			}
		}

		static string GetStoredTutorials()
		{
			if (System.IO.File.Exists(TutorialTempFile))
			{
				System.IO.StreamReader sr = System.IO.File.OpenText(TutorialTempFile);
				try 
				{
					return sr.ReadToEnd();
				} 
				finally 
				{
					sr.Close();
					sr.Dispose();
					sr = null;
				}
			}

			return "";
		}

		static void SaveTutorials(string cont)
		{
			System.IO.StreamWriter sw = System.IO.File.CreateText(TutorialTempFile);
			try 
			{
				sw.Write(cont);
			} 
			finally 
			{
				sw.Close();
				sw.Dispose();
				sw = null;
			}
		}

		static string TazzMannTutorial(bool real)
		{
			if (real) return System.IO.Path.Combine(Helper.SimPePath, @"Doc\SimPE_FTGU.pdf");
			else return "http://localhost/Doc/SimPE_FTGU.pdf";
		}

		static string Introduction(bool real)
		{
			if (real) return System.IO.Path.Combine(Helper.SimPePath, @"Doc\Introduction.pdf");
			else return "http://localhost/Doc/Introduction.pdf";
		}

		/// <summary>
		/// Display the Update Screen
		/// </summary>
		/// <param name="show">true, if it should be visible even if no updates were found</param>
		public static void ShowTutorials()
		{
			Wait.SubStart();
			About f = new About(true);
			string text = "";
            string html = GetHtmlBase();
			try 
			{
				f.Title = SimPe.Localization.GetString("Tutorials");

				text += "<p>";
				if (System.IO.File.Exists(Introduction(true)))
				{
					text += "\n                <li>";
					text += "\n                    <a href=\""+Introduction(false)+"\"><span class=\"serif\">Emily:</span> Introduction to the new SimPE</a>";
					text += "\n                </li>";
				}
				if (System.IO.File.Exists(TazzMannTutorial(true)))
				{
					text += "\n                <li>";
					text += "\n                    <a href=\""+TazzMannTutorial(false)+"\"><span class=\"serif\">TazzMann:</span> SimPE - From the Ground Up</a>";
					text += "\n                </li>";
				}
				text += "</p>";

                // f.wb.DocumentText not available in Avalonia port
                SaveTutorials(text);
				f.rtb.Text = text;
			}
			catch (Exception ex)
			{
				f.rtb.Text = GetStoredTutorials();
                if (string.IsNullOrEmpty(f.rtb.Text))
                {
                    f.rtb.Text = ex.Message;
                }
			}

            Wait.SubStop();
            SimPe.Splash.Screen.Stop();
			f.ShowDialog();
		}

		private void rtb_LinkClicked(object sender, EventArgs e)
		{
			// TODO: link handling not ported
		}

		private void rtb_Enter(object sender, System.EventArgs e)
		{
			button1.Focus();
		}

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
	}
}
