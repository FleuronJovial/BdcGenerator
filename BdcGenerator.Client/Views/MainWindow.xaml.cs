using BdcGenerator.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BdcGenerator.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string NoModelLabel = "Aucun modèle sélectionné";
        private const string SelectedModelLabel = "Modèle: {0}";
        private const string NoPhotoLabel = "Aucun dossier photo sélectionné";
        private const string SelectedPhotoLabel = "{0} ({1} images)";
        private const string GeneratedLabel = "{0} fichiers créés";

        private MainWindowVM _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowVM();
            DataContext = _vm;
        }

        private void OnModelButtonClicked(object sender, RoutedEventArgs e)
        {
            var openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "Word Open XML (*.docx)|*.docx";

            var result = openFileDlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var path = openFileDlg.FileName;
                _vm.ModelPath = path;

                var folderName = System.IO.Path.GetFileName(path);

                LblModel.Text = String.Format(SelectedModelLabel, folderName);
                LblModel.Background = new SolidColorBrush(Colors.LightGreen);
            }

            EnableStartButton();
            //if (result.ToString() != string.Empty)
            //{
            //    //txtPath.Text = openFileDlg.SelectedPath;
            //}
            //root = txtPath.Text;
        }

        private void EnableStartButton()
        {
            GenerateBtn.IsEnabled = LblModel.Text != NoModelLabel && lblPhoto.Text != NoPhotoLabel;
        }
        private void OnGenerateButtonClicked(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFileDlg = new FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var outputPath = openFileDlg.SelectedPath;

                var r = _vm.GenerateFiles(outputPath);

                LblGenerate.Text = string.Format(GeneratedLabel, r.FileCount);
                Process.Start(r.OutputFolder);
            }
        }
        private void OnImagesButtonClicked(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFileDlg = new FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var path = openFileDlg.SelectedPath;

                _vm.PhotoFolder = path;
                var folderName = System.IO.Path.GetFileName(path);

                int imageFileCount = _vm.CountImageFiles(path);
                lblPhoto.Text = String.Format(SelectedPhotoLabel, folderName, imageFileCount);

                if (imageFileCount > 0)
                {
                    lblPhoto.Background = new SolidColorBrush(Colors.LightGreen);
                }
                else
                {
                    lblPhoto.Background = new SolidColorBrush(Colors.Orange);
                }
            }
            EnableStartButton();
        }



    }
}
