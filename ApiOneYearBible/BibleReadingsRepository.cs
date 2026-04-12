using ApiOneYearBible.Models;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiOneYearBible;

public class BibleReadingsRepository : IBibleReadingsRepository
{
    private readonly BibleReadings repo;
    static  readonly HttpClient    client = new HttpClient();
    
    public BibleReadingsRepository()
    {
        repo = new BibleReadings();
    }

    private static readonly string[] MonthFilePrefix =
    {
        "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
    };

    public async Task<BibleReadings> GetAllBibleReadings(DateOnly date)
    {
        // getting the day of year will help index for that day's readings
        //DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        // get the day of the current year
        string monthPrefix = MonthFilePrefix[date.Month];
       
        // The "month" text files contain all the readings for each day/month
        string fileLoc = Path.Combine(AppContext.BaseDirectory, "..", "..", "..","wwwroot", $"{monthPrefix}Verses.txt");
        string line = File.ReadLines(fileLoc).ElementAtOrDefault(date.Day - 1);

        // Delimiter is semicolon
        string[] readings = line.Split(';');

        repo.monthDay = readings[0];
        repo.OldTestamentVerses = readings[1];
        repo.NewTestamentVerses = readings[2];
        repo.PsalmVerses   = readings[3];
        repo.ProverbVerses = readings[4];
       
        for (var i = 1; i <= 4; i++)
        {
            // A reading may cross books. In these cases, the different books are fetched separately,
            // and separated by a comma.
            string[] passages = readings[i].Split(',');
            var textParts = new List<string>();
            
            foreach (string passage in passages)
            {
                // comment that follows shows a fetch crossing books... one of the hardest examples
                // string url = $"https://labs.bible.org/api/?passage=Gen+50:1-26+Exo:1:1-2:10";
                
                // Deuteronomy28+1-68;Luke11+14-36;Psalm77+1-20;Pro12+18
                string url = $"https://labs.bible.org/api/?passage={passage}&formatting=full";
                
                try
                {
                    string bibleApiResponse = await client.GetStringAsync(url);
                    
                    textParts.Add(bibleApiResponse);
                  
                    // Console.WriteLine(bibleApiResponse); for debugging purposes
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error fetching [{url}]: {e.Message}");
                }
            }
            repo.ApiText[i - 1] = string.Join(" ", textParts);
        }
        return repo;
    }
    
} // End BibleReadingsRepository class