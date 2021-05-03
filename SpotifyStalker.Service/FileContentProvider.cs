using SpotifyStalker.Interface;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpotifyStalker.Service
{
    public class FileContentProvider : IFileContentProvider
    {
        public string Get(string fileName)
        {
            var targetFile =
                Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), fileName);
            // ReadAllTextAsync wasn't working consistently. Don't know why. Didn't want to spend
            // time figuring it out.
            return File.ReadAllText(targetFile);
        }

        public IEnumerable<string> GetEnumerable(string fileName)
        {
            var fileData = Get(fileName);

            using var sr = new StringReader(fileData);
            var line = string.Empty;
            while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                yield return line;
        }
    }
}
