using System.Collections.Generic;

namespace SpotifyStalker.Interface
{
    public interface IFileContentProvider
    {
        string Get(string fileName);

        IEnumerable<string> GetEnumerable(string fileName);
    }
}
