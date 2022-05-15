using MvvmMicro;
using System;

namespace BdcGenerator.Client.ViewModels
{
    internal class MainWindowVM : ObservableObject
    {
        public MainWindowVM()
        {
            _generator = new Generator();
        }

        private readonly Generator _generator;

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

        internal int CountImageFiles(string path)
        {
            return _generator.CountImageFiles(path);
        }

        internal GenerationResponse GenerateFiles(string outputPath)
        {
            return _generator.GenerateAsync(new GenerationRequest
            {
                ModelPath = ModelPath,
                PhotoFolder = PhotoFolder,
                OutputFolder = outputPath
            }).GetAwaiter().GetResult();

       
        }
    }
}
