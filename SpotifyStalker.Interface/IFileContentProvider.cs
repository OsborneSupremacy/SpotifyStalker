using System.Collections.Generic;

namespace SpotifyStalker.Interface;

public interface IFileContentProvider
{
    string Get(string directoryName, string fileName);

    IEnumerable<string> GetEnumerable(string directoryName, string fileName);
}
