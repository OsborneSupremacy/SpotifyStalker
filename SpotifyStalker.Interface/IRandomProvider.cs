﻿namespace SpotifyStalker.Interface;

public interface IRandomProvider
{
    public string GetPersonName();

    public string GetWord();

    public string GetLocation();

    public string GetArtist();

    public string GetGenre();

    T PickRandom<T>(IEnumerable<T> items);
}
