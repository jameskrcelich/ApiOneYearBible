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

    public async Task<BibleReadings> GetAllBibleReadings()
    {
        // getting the day of year will help index for that day's readings
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        // get the day of the current year
        string monthPrefix = MonthFilePrefix[today.Month];
       
        // The "month" text files contain all the readings for each day/month
        string fileLoc = Path.Combine(AppContext.BaseDirectory, "..", "..", "..","wwwroot", $"{monthPrefix}Verses.txt");
        string line    = File.ReadLines(fileLoc).ElementAtOrDefault(today.Day - 1);

        // Delimiter is semicolon
        string[] readings = line.Split(';');

        repo.monthDay = readings[0];
        repo.OldTestamentVerses = readings[1];
        repo.NewTestamentVerses = readings[2];
        repo.PsalmVerses   = readings[3];
        repo.ProverbVerses = readings[4];
       
        for (var i = 1; i <= 4; i++)
        {
            // A reading may contain multiple comma-separated passages (e.g. "Deu2:1-37,Deu3:1-29")
            // bible-api.com does not support cross-passage commas, so fetch each one separately
            string[] passages = readings[i].Split(',');
            var textParts = new List<string>();
            
            foreach (string passage in passages)
            {
                //string url = $"https://bible-api.com/{passage.Trim()}?translation=asv";
                //string url = $"https://labs.bible.org/api/?passage=Gen+50:26+Exo:1:1-5";
                
                //Deu28+1-68;Luk11+14-36;Psalm77+1-20;Pro12+18 */
                string url = $"https://labs.bible.org/api/?passage={passage}&formatting=full";
                
                try
                {
                    string bibleApiResponse = await client.GetStringAsync(url);
                    
                    textParts.Add(bibleApiResponse);
                  
                    Console.WriteLine(bibleApiResponse);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error fetching [{url}]: {e.Message}");
                }
            }
            repo.ApiText[i - 1] = string.Join(" ", textParts);
        }
        
        // Print all the daily readings.
        /* Console.WriteLine($"{readings[1]}, {repo.ApiText[0]}");
        Console.WriteLine($"{readings[2]}, {repo.ApiText[1]}");
        Console.WriteLine($"{readings[3]}, {repo.ApiText[2]}");
        Console.WriteLine($"{readings[4]}, {repo.ApiText[3]}"); */
        
        return repo;
    }
    
    /*[JSInvokable]
    public static Task<BibleReadings> GetBibleReadings( DateOnly clickedDate )
    {
        string monthPrefix = MonthFilePrefix[clickedDate.Month];
       
        // The "month" text files contain all the readings for each day/month
        string fileLoc = Path.Combine(AppContext.BaseDirectory, "..", "..", "..","wwwroot", $"{monthPrefix}Verses.txt");
        string line    = File.ReadLines(fileLoc).ElementAtOrDefault(clickedDate.Day - 1);

        // Delimiter is semicolon
        string[] readings = line.Split(';');

        repo.monthDay = readings[0];
        repo.OldTestamentVerses = readings[1];
        repo.NewTestamentVerses = readings[2];
        repo.PsalmVerses   = readings[3];
        repo.ProverbVerses = readings[4];
       
        for (var i = 1; i <= 4; i++)
        {
            // A reading may contain multiple comma-separated passages (e.g. "Deu2:1-37,Deu3:1-29")
            // bible-api.com does not support cross-passage commas, so fetch each one separately
            string[] passages = readings[i].Split(',');
            var textParts = new List<string>();
            
            foreach (string passage in passages)
            {
                //string url = $"https://bible-api.com/{passage.Trim()}?translation=asv";
                //string url = $"https://labs.bible.org/api/?passage=Gen+50:26+Exo:1:1-5";
                
                //Deu28+1-68;Luk11+14-36;Psalm77+1-20;Pro12+18 
                string url = $"https://labs.bible.org/api/?passage={passage}&formatting=full";
                
                try
                {
                    string bibleApiResponse = await client.GetStringAsync(url);
                    
                    textParts.Add(bibleApiResponse);
                  
                    Console.WriteLine(bibleApiResponse);
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
        */
}