using MvvmMicro;

namespace BdcGenerator.Client.ViewModels
{
    internal class MainWindowVM : ObservableObject
    {
        private string _photoFolder;
        public string PhotoFolder
        {
            get { return _photoFolder; }
            set { Set(ref _photoFolder, value); }
        }

        private string _modelPath;
        public string ModelPath
        {
            get { return _modelPath; }
            set { Set(ref _modelPath, value); }
        }


    }
}
