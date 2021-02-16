using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyStalker.ConsoleUi
{
    public class ArtistQueryService
    {
        private readonly IConfiguration _configuration;

        private readonly string[] _searchCharacters =
        {
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "!",
            "@",
            "#",
            "$",
            "%",
            "^",
            "&",
            "*",
            "(",
            ")",
            "+",
            "=",
            "?",
            " "
        };

        public ArtistQueryService(
            IConfiguration configuration
        )
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        public Task ExecuteAsync()
        {
            Console.WriteLine("Doing something");

            var permutations = GetSearchTermPermutations();

            foreach(var p in permutations)
                Console.WriteLine(p);

            return Task.CompletedTask;
        }

        protected HashSet<string> GetSearchTermPermutations()
        {
            var permutations1 = new List<string>();
            permutations1.AddRange(_searchCharacters);

            var permutations2 = new List<string>();

            foreach(var p in permutations1)
                permutations2.AddRange(_searchCharacters.Select(x => $"{p}{x}"));

            var permutations3 = new List<string>();

            foreach (var p in permutations2)
                permutations3.AddRange(_searchCharacters.Select(x => $"{p}{x}"));

            return permutations1
                .Union(permutations2)
                .Union(permutations3)
                .ToHashSet();
        }

    }
}
