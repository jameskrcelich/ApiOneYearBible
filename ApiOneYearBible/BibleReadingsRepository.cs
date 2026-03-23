using ApiOneYearBible.Models;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ApiOneYearBible;

public class BibleReadingsRepository : IBibleReadingsRepository
{
    private readonly BibleReadings repo;
    private readonly DateOnly _dateOnly;
    static  readonly HttpClient client = new HttpClient();
    
    public BibleReadingsRepository(BibleReadings bibleReadings)
    {
        repo = bibleReadings;
    }

    public BibleReadings GetAllBibleReadings()
    {
        // getting the day of year will help index for that day's readings
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        // get the day of the current year
        int day = today.Day;

        string fileLoc = @"C:\Users\jimkr\OneDrive\Desktop\repos\ApiOneYearBible\ApiOneYearBible\wwwroot\MarVerses.txt";
        string line = File.ReadLines(fileLoc).ElementAtOrDefault(day - 1);

        // Delimiter is semicolon
        string[] readings = line.Split(';');

        repo.OldTestamentVerses = readings[0];
        repo.NewTestamentVerses = readings[1];
        repo.PsalmVerses = readings[2];
        repo.ProverbVerses = readings[3];
        repo.monthDay = readings[4];
    
        Console.WriteLine($"{readings[1]}, {readings[2]}, {readings[3]}, {readings[4]}");

        //Console.WriteLine($"{values[3]}");
        //System.Diagnostics.Debug.WriteLine("My debug message here");

        dynamic data = "";

        for (var i = 0; i <= 4; i++)
        {
            string url = $"https://bible-api.com/{readings[i]}?translation=kjv";

            var bibleApiResponse = client.GetStringAsync(url).Result;

            // Process the JSON response (example using a simple dynamic approach)
            data = JsonConvert.DeserializeObject(bibleApiResponse);

            repo.ApiText[i] = string.Copy(data.text);
        }

        // Console.WriteLine($"Passage: {data.reference}");
        // Console.WriteLine($"Text: {data.text}");
        // Console.WriteLine($"Text: {bibleApiResponse}");

        return repo;
    }
}