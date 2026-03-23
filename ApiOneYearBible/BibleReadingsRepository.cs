using ApiOneYearBible.Models;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ApiOneYearBible;

public class BibleReadingsRepository : IBibleReadingsRepository
{
    private readonly BibleReadings repo;
    //private readonly DateOnly _dateOnly;
    static  readonly HttpClient client = new HttpClient();
    
    public BibleReadingsRepository()
    {
        repo = new BibleReadings();
    }

    public BibleReadings GetAllBibleReadings()
    {
        // getting the day of year will help index for that day's readings
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        // get the day of the current year
        int day = today.Day;

        string fileLoc = Path.Combine(AppContext.BaseDirectory, "..", "..", "..","wwwroot", "MarVerses.txt");
        string line = File.ReadLines(fileLoc).ElementAtOrDefault(day - 1);

        // Delimiter is semicolon
        string[] readings = line.Split(';');

        repo.OldTestamentVerses = readings[1];
        repo.NewTestamentVerses = readings[2];
        repo.PsalmVerses = readings[3];
        repo.ProverbVerses = readings[4];
        repo.monthDay = readings[0];
    
        Console.WriteLine($"{readings[1]}, {readings[2]}, {readings[3]}, {readings[4]}");

        //Console.WriteLine($"{values[3]}");
        //System.Diagnostics.Debug.WriteLine("My debug message here");

        dynamic data = "";

        for (var i = 1; i <= 4; i++)
        {
            //string url = $"https://bible-api.com/{readings[i]}?translation=kjv";
            string url = $"https://bible-api.com/Pro11:24-26?translation=kjv\";?translation=kjv";

            var bibleApiResponse = client.GetStringAsync(url).Result;

            // Process the JSON response (example using a simple dynamic approach)
            data = JsonConvert.DeserializeObject(bibleApiResponse);

            repo.ApiText[i-1] = string.Copy(data.text);
        }

        // Console.WriteLine($"Passage: {data.reference}");
        // Console.WriteLine($"Text: {data.text}");
        // Console.WriteLine($"Text: {bibleApiResponse}");

        return repo;
    }
}