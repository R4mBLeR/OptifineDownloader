using System.Diagnostics;
using System.Windows;
using OptifineDownloader.Utils.StringManager;
using OptifineDownloader.Utils.Parser;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.WindowsAPICodePack.Dialogs;
using OptifineDownloader.Utils;

namespace OptifineDownloader
{
    public partial class MainWindow : Window
    {
        private const string url = "https://optifine.net/downloads";
        private readonly Dictionary<string, List<string>>? versions;
        private object locker = new();
        public MainWindow()
        {
            InitializeComponent();
            versions = GetVersionsDictionary();
            Versions_ComboBox.ItemsSource = versions.Keys.ToArray();
            Versions_ComboBox.SelectedItem = Versions_ComboBox.Items[0];
            ResetOptiFineVersions();
            DownloadPatch_Field.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Run_CheckBox.IsChecked = false;
        }

        private void Versions_ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ResetOptiFineVersions();
        }

        private async void Download_Button_Click(object sender, RoutedEventArgs e)
        {
            string downloadPageUrl = StringManager.GetDownloadPageUrl(Versions_ComboBox.Text,Optifine_Versions_ComboBox.Text);
            string html = WebHelper.GetHtml(downloadPageUrl);
            Debug.WriteLine(html);
            string downloadLink = Parser.GetFinalDownloadLink(html);
            string downloadName = StringManager.GetDownloadName(Versions_ComboBox.Text, Optifine_Versions_ComboBox.Text);
            DownloadInfo_Label.Content = "Optifine is downloading";
            SetEnabledDownloadElements(false);
            await WebHelper.DownloadOptifine(downloadLink, DownloadPatch_Field.Text, downloadName);
            SetEnabledDownloadElements(true);
            DownloadInfo_Label.Content = "Optifine downloaded!";
            if (Run_CheckBox.IsChecked == true)
            {
                string command;
                command = $"-jar \"{DownloadPatch_Field.Text}\\{downloadName}.jar\"";
                var processInfo = new ProcessStartInfo("java.exe", command)
                {
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                try
                {
                    var proc = Process.Start(processInfo);
                    proc!.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

        }

        private void DownloadPatch_Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new()
            {
                IsFolderPicker = true,
                Multiselect = true
            };
            
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DownloadPatch_Field.Text = dialog.FileName;
            }
        }

        private void SetEnabledDownloadElements(bool value)
        {
            DownloadPatch_Field.IsEnabled = value;
            DownloadPatch_Button.IsEnabled = value;
            Download_Button.IsEnabled = value;
            Run_CheckBox.IsEnabled = value;
        }

        private Dictionary<string, List<string>> GetVersionsDictionary()
        {
            string html = WebHelper.GetHtml(url);
            string[] downloadLinks = Parser.GetDownloadLinks(html);
            string[] usortedVersions = StringManager.RemoveExcess(downloadLinks);
            Dictionary<string, List<string>> versions = StringManager.GroupingByVersions(usortedVersions);
            return versions;
        }
        
        private void ResetOptiFineVersions()
        {
            Optifine_Versions_ComboBox.ItemsSource = versions![Versions_ComboBox.SelectedItem.ToString()!];
            Optifine_Versions_ComboBox.SelectedItem = Optifine_Versions_ComboBox.Items[0];
            if (Optifine_Versions_ComboBox.Text.IndexOf("pre") > -1)
            {
                for (int i = 0; i < Optifine_Versions_ComboBox.Items.Count; i++)
                {
                    if (Optifine_Versions_ComboBox.Items[i].ToString()!.IndexOf("pre") == -1)
                    {
                        Optifine_Versions_ComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
    }
}
