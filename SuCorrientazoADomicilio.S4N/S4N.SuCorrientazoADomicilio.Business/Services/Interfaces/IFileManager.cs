using System;
using System.Collections.Generic;
using System.Text;

namespace S4N.SuCorrientazoADomicilio.Business.Services.Interfaces
{
    public interface IFileManager
    {
        /// <summary>
        /// Write drone results
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="routeText">The file content text</param>
        /// <param name="path">The output directory path</param>
        void WriteFile(string fileName, StringBuilder routeText, string path = "");

        /// <summary>
        /// Read a file in the provided path and returns the content
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="path">The output directory path</param>
        /// <returns>List of instructions</returns>
        List<string> ReadFile(string fileName, string path = "");
    }
}
