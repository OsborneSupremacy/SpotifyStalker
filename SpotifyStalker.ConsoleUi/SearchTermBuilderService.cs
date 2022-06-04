using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SpotifyStalker.ConsoleUi;

public class SearchTermBuilderService
{
    private readonly ILogger<SearchTermBuilderService> _logger;

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

    public SearchTermBuilderService(ILogger<SearchTermBuilderService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public HashSet<string> GenerateSearchTerms()
    {
        var searchTerms = new List<string>();
        searchTerms.AddRange(_searchCharacters);
        searchTerms.AddRange(CrossJoin(_searchCharacters, _searchCharacters));

        var uniqueSearchTerms = searchTerms.ToHashSet();
        _logger.LogInformation($"Unique search terms generated: {uniqueSearchTerms.Count}");
        return uniqueSearchTerms;
    }

    protected List<string> CrossJoin(IEnumerable<string> listOne, IEnumerable<string> listTwo) =>
        listOne
            .SelectMany(x => listTwo, (x, y) => { return $"{x}{y}"; })
            .ToList();
}
