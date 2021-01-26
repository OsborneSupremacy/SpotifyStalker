using SpotifyStalker.Interface;
using Bogus;
using System.Collections.Generic;

namespace SpotifyStalker.Service
{
    public class RandomProvider : IRandomProvider
    {
        public string GetWord() => new Randomizer().Word().ToString();

        public T PickRandom<T>(IEnumerable<T> items) => new Faker().PickRandom(items);

        public string GetLocation() => new Faker().Address.City().ToString();
    }
}
