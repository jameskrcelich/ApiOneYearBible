using System.Collections.Generic;
using ApiOneYearBible.Models;

namespace ApiOneYearBible;

public interface IBibleReadingsRepository
{
    public Task<BibleReadings> GetAllBibleReadings(DateOnly date);
    //public Task<BibleReadings> GetBibleReadings(DateOnly date);
}