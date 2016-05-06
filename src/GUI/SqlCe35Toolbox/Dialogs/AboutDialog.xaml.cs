﻿using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System;
using Microsoft.VisualStudio.PlatformUI;
using System.ComponentModel;
using ErikEJ.SqlCeToolbox.Helpers;
using System.IO;

namespace ErikEJ.SqlCeToolbox.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateLoginDialog.xaml
    /// </summary>
    public partial class AboutDialog : DialogWindow
    {

        public AboutDialog()
        {
            InitializeComponent();
            Telemetry.TrackPageView(nameof(AboutDialog));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SqlCeToolboxPackage.VisualStudioVersion < new Version(11, 0))
            {
                DDEXButton.Visibility = System.Windows.Visibility.Collapsed;
            }
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += (s, ea) =>
            {
                Version.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version + " " + ea.Result.ToString();
            };
            bw.RunWorkerAsync();

            this.Background = Helpers.VSThemes.GetWindowBackground();
            Version.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version;

            txtStatus.Text = "SQL Server Compact 4.0 in GAC - ";
            try
            {
                Assembly asm4 = System.Reflection.Assembly.Load("System.Data.SqlServerCe, Version=4.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91");
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm4.Location); 
                string version = fvi.FileVersion;
                txtStatus.Text += string.Format("Yes - {0}\n", version.ToString());
            }
            catch (System.IO.FileNotFoundException)
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "SQL Server Compact 4.0 DbProvider - ";
            try
            {
                var factory = System.Data.Common.DbProviderFactories.GetFactory(ErikEJ.SqlCeToolbox.Resources.SqlCompact40InvariantName);
                txtStatus.Text += "Yes\n";
            }
            catch (System.Configuration.ConfigurationException)
            {
                txtStatus.Text += "No\n";
            }
            catch (ArgumentException)
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "\nSQL Server Compact 4.0 DDEX provider - ";
            try
            {
                if (Helpers.DataConnectionHelper.DdexProviderIsInstalled(new Guid(ErikEJ.SqlCeToolbox.Resources.SqlCompact40Provider)))
                {
                    txtStatus.Text += "Yes\n";
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "SQL Server Compact 4.0 Simple DDEX provider - ";
            try
            {
                if (Helpers.DataConnectionHelper.DdexProviderIsInstalled(new Guid(ErikEJ.SqlCeToolbox.Resources.SqlCompact40PrivateProvider)))
                {
                    txtStatus.Text += "Yes\n";
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }

            var tempFile40 = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            var tempFile35 = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            try
            {
                var helper = Helpers.DataConnectionHelper.CreateEngineHelper(DatabaseType.SQLCE40);
                var conn40 = string.Format("Data Source={0}", tempFile40);
                helper.CreateDatabase(conn40);
                txtStatus.Text += "SQL Server Compact 4.0 Engine test: PASS!\n";
            }
            catch
            {
                txtStatus.Text += "SQL Server Compact 4.0 Engine test: FAIL!\n";
            }

            txtStatus.Text += "\n\nSQL Server Compact 3.5 in GAC - ";
            try
            {
                Assembly asm35 = System.Reflection.Assembly.Load("System.Data.SqlServerCe, Version=3.5.1.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91");
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm35.Location);
                string version = fvi.FileVersion;
                txtStatus.Text += string.Format("Yes - {0}\n", version.ToString());
            }
            catch (System.IO.FileNotFoundException)
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "SQL Server Compact 3.5 DbProvider - ";
            try
            {
                var factory = System.Data.Common.DbProviderFactories.GetFactory(ErikEJ.SqlCeToolbox.Resources.SqlCompact35InvariantName);
                txtStatus.Text += "Yes\n";
            }
            catch (System.Configuration.ConfigurationException)
            {
                txtStatus.Text += "No\n";
            }
            catch (ArgumentException)
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "\nSQL Server Compact 3.5 DDEX provider - ";
            try
            {
                if (Helpers.DataConnectionHelper.DdexProviderIsInstalled(new Guid(ErikEJ.SqlCeToolbox.Resources.SqlCompact35Provider)))
                {
                    txtStatus.Text += "Yes\n";
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "SQL Server Compact 3.5 Simple DDEX provider - ";
            try
            {
                if (Helpers.DataConnectionHelper.DdexProviderIsInstalled(new Guid(ErikEJ.SqlCeToolbox.Resources.SqlCompact35PrivateProvider)))
                {
                    txtStatus.Text += "Yes\n";
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }
            try
            {
                var helper = Helpers.DataConnectionHelper.CreateEngineHelper(DatabaseType.SQLCE40);
                var conn35 = string.Format("Data Source={0}", tempFile35);
                helper.CreateDatabase(conn35);
                txtStatus.Text += "SQL Server Compact 3.5 Engine test: PASS!\n";
            }
            catch
            {
                txtStatus.Text += "SQL Server Compact 3.5Engine test: FAIL!\n";
            }

            txtStatus.Text += "\n\nSync Framework 2.1 SqlCe 3.5 provider - ";
            if (Helpers.DataConnectionHelper.IsSyncFx21Installed())
            {
                txtStatus.Text += "Yes\n";
            }
            else
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "\n\nSQLite ADO.NET Provider included: ";
            try
            {
                Assembly asm = System.Reflection.Assembly.Load("System.Data.SQLite");
                txtStatus.Text += string.Format("{0}\n", asm.GetName().Version);
            }
            catch (System.IO.FileNotFoundException)
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "SQLite EF6 DbProvider in GAC - ";
            try
            {
                if (Helpers.DataConnectionHelper.IsSqLiteDbProviderInstalled())
                {
                    txtStatus.Text += "Yes\n";
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }

            try
            {
                if (File.Exists(tempFile40))
                {
                    File.Delete(tempFile40);
                }
                if (File.Exists(tempFile35))
                {
                    File.Delete(tempFile35);
                }
            }
            catch { }
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = Helpers.DataConnectionHelper.GetDownloadCount();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //Helpers.DataConnectionHelper.LogError(new Exception(Guid.NewGuid().ToString()));
            Close();
        }

        private void CodeplexLink_Click(object sender, RoutedEventArgs e)
        {
            Helpers.EnvDTEHelper.LaunchUrl("http://sqlcetoolbox.codeplex.com");
        }

        private void DDEXButton_Click(object sender, RoutedEventArgs e)
        {
            Helpers.DataConnectionHelper.RegisterDdexProviders(true);
            EnvDTEHelper.ShowMessage("Providers registered, you may have to restart Visual Studio");
        }

        private void GalleryLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://visualstudiogallery.msdn.microsoft.com/0e313dfd-be80-4afb-b5e9-6e74d369f7a1/view/Reviews");
        }
    }
}