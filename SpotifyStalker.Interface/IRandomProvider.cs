using System.Collections.Generic;

namespace SpotifyStalker.Interface
{
    public interface IRandomProvider
    {
        public string GetWord();

        public string GetLocation();

        T PickRandom<T>(IEnumerable<T> items);
    }
}
