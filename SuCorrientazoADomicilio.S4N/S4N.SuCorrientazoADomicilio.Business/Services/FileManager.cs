using S4N.SuCorrientazoADomicilio.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace S4N.SuCorrientazoADomicilio.Business.Services
{
    public class FileManager : IFileManager
    {
        /// <summary>
        /// Write drone results
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="routeText">The file content text</param>
        /// <param name="path">The output directory path</param>
        public void WriteFile(string fileName, StringBuilder routeText, string path = "")
        {
            try
            {
                var targetPath = GetSourcePath(path, "OutputFiles");
                var fullPath = Path.Combine(targetPath, fileName);

                var directory = new DirectoryInfo(Path.GetFullPath(targetPath));
                if (!directory.Exists)
                {
                    var info = Directory.CreateDirectory(targetPath);
                    var value = info.GetDirectories();
                }

                using (var outputFile = new StreamWriter(fullPath))
                {
                    outputFile.WriteLine(routeText.ToString());
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error writing file.", exception);
            }
        }

        /// <summary>
        /// Read a file in the provided path and returns the content
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="path">The input directory path</param>
        /// <returns>List of instructions</returns>
        public List<string> ReadFile(string fileName, string path = "")
        {
            try
            {
                var sourcePath = GetSourcePath(path, "InputFiles");
                var fullPath = Path.Combine(sourcePath, fileName);

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"File {fileName} does not exists");
                    return new List<string>();
                }

                var lines = File.ReadAllLines(fullPath);

                return lines.Select(line => line.Replace("\"", "")).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception("Error reading file.", exception);
            }
        }

        #region Private methods
        private static string GetSourcePath(string inputPath, string folderName)
        {
            var path = (string.IsNullOrEmpty(inputPath)
                ? Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)
                : inputPath);

            return Path.Combine(path, folderName);
        }
        #endregion
    }
}
