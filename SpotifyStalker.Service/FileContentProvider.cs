using SpotifyStalker.Interface;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpotifyStalker.Service
{
    public class FileContentProvider : IFileContentProvider
    {
        public string Get(string directoryName, string fileName)
        {
            var targetDirectoryName = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), directoryName);

            // in the docker container, the folder/file isn't necessarily in the same place
            // as it is when running not in a container, so look around for the directory as necessary
            var directoryInfo = new DirectoryInfo(targetDirectoryName);
            while (!directoryInfo.Exists && directoryInfo.Parent != null)
            {
                directoryInfo = directoryInfo.Parent;
                directoryInfo.Refresh();
            }

            var targetFile = Path.Combine(directoryInfo.FullName, fileName);

            // ReadAllTextAsync wasn't working consistently. Don't know why. Didn't want to spend
            // time figuring it out.
            return File.ReadAllText(targetFile);
        }

        public IEnumerable<string> GetEnumerable(string directoryName, string fileName)
        {
            var fileData = Get(directoryName, fileName);

            using var sr = new StringReader(fileData);
            var line = string.Empty;
            while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                yield return line;
        }
    }
}
