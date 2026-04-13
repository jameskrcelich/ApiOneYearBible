using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiOneYearBible.Models;

public class BibleReadings
{
    public string OldTestamentVerses { get; set ; }
    
    public string NewTestamentVerses { get; set ; }
    
    public string PsalmVerses { get; set ; }
    
    public string ProverbVerses { get; set ; }

    public string[] ApiText { get; set; } = new string[4];
    
    public string monthDay { get; set; }
}