using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateEngine.Docx;

namespace BdcGenerator
{
    public interface IGenerator
    {
        int CountImageFiles(string path);
        Task GenerateAsync(GenerationRequest request);
    }
    public class Generator : IGenerator
    {
        private static readonly string[] ImageExtensions = new[] { ".jpg", ".png" };

        public async Task GenerateAsync(GenerationRequest request)
        {
            var outputPath = request.OutputFolder;
            var modelFileName = System.IO.Path.GetFileName(request.ModelPath);

            var outFileName = System.IO.Path.Combine(outputPath, modelFileName);

            File.Delete(outFileName);
            File.Copy(request.ModelPath, outFileName);

            var valuesToFill = new Content(
           new FieldContent("Référence", "Toto was here :)"));


            using (var outputDocument = new TemplateProcessor(outFileName)
                .SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
        }

        public int CountImageFiles(string path)
        {
            var d = new System.IO.DirectoryInfo(path);

            var files = d.GetFiles();


            return files.Count(x => IsImageFileName(x.Extension));
        }

        private static bool IsImageFileName(string filename)
        {
            return ImageExtensions.Any(x => x.Equals(filename, StringComparison.OrdinalIgnoreCase));
        }
    }
}
