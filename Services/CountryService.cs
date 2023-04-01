using Application.Services.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.ModelContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly CountryContext _context;
        private readonly ILogger<CountryService> _logger;

        public CountryService(CountryContext context, ILogger<CountryService> logger) 
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<Country>> GetCountriesAsync()
        {
            var countries = await _context.CountryContexts.Select(c => new Country
            {
                Id = c.Id,
                Name = c.Name,
                CapitalCity = c.CapitalCity,
                Lattitude = c.Lattitude,
                Longitude = c.Longitude,
                PopulationCount = c.PopulationCount,
                ShortCode = c.ShortCode,
                OwnedRegions = c.OwnedRegions.Select(r => new Regions
                {
                    Id = r.Id,
                    Name = r.Name,
                    ShortCode = r.ShortCode,
                    CountryId = r.CountryId

                }).ToList()
            }).ToListAsync();

            return countries;
        }
    }
}
