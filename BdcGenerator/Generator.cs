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
        Task<GenerationResponse> GenerateAsync(GenerationRequest request);
    }
    public class Generator : IGenerator
    {
        private static readonly string[] ImageExtensions = new[] { ".jpg", ".png" };
        private const string ReferenceTag = "Reference";
        private const string PictureTag = "Picture";

        public async Task<GenerationResponse> GenerateAsync(GenerationRequest request)
        {
            var outputPath = request.OutputFolder;
            var modelFileName = System.IO.Path.GetFileName(request.ModelPath);

            var pictureFolder = new System.IO.DirectoryInfo(request.PhotoFolder);
            var pictureFiles = pictureFolder.GetFiles().Where(x => IsImageFileName(x.Extension));

            var generatedCount = 0;

            foreach (var pictureFile in pictureFiles)
            {
                var imageName = pictureFile.Name.Substring(0, pictureFile.Name.Length - pictureFile.Extension.Length);

                var templateFileName = System.IO.Path.GetFileName(request.ModelPath);

                var outFileName = templateFileName.Contains("Template")
                   ? templateFileName.Replace("Template", imageName)
                   : $"{templateFileName}_{imageName}.docx";

                var outFilePath = System.IO.Path.Combine(outputPath, outFileName );

                File.Delete(outFilePath);
                File.Copy(request.ModelPath, outFilePath);

                var valuesToFill = new Content(
                   new FieldContent(ReferenceTag, imageName),
                   new ImageContent(PictureTag, File.ReadAllBytes(pictureFile.FullName))
                   );

                using (var outputDocument = new TemplateProcessor(outFilePath)
                    .SetRemoveContentControls(true))
                {
                    outputDocument.FillContent(valuesToFill);
                    outputDocument.SaveChanges();
                }

                generatedCount++;
            }



            return new GenerationResponse
            {
                OutputFolder = outputPath,
                FileCount = generatedCount
            };
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
