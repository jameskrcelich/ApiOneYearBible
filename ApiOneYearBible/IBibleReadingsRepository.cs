using System.Collections.Generic;
using ApiOneYearBible.Models;

namespace ApiOneYearBible;

public interface IBibleReadingsRepository
{
    public BibleReadings GetAllBibleReadings();
    //BibleReadings GetBibleReadings(DateOnly date);
}