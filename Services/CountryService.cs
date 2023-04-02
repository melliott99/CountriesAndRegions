using Application.Services.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Repository.ModelContext;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly CountryContext _context;
        private readonly ILogger<CountryService> _logger;
        private readonly IMemoryCache _cache;

        public CountryService(CountryContext context, ILogger<CountryService> logger, IMemoryCache cache) 
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        /// <inheritdoc/>
        public async Task<List<Country>> GetCountriesAsync()
        {
            var cacheKey = "all_countries";
            if (_cache.TryGetValue(cacheKey, out List<Country> cachedCountries))
            {
                return cachedCountries;
            }

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
            }).AsNoTracking().ToListAsync();

            CacheResults(countries, cacheKey);

            return countries;
        }

        /// <inheritdoc/>
        public async Task<Country> GetCountriesByNameAsync(string name)
        {
            Country? country = null;

            country = await _context.CountryContexts.Select(c => new Country
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
            }).AsNoTracking().FirstOrDefaultAsync(c => name.Equals(c.Name));
            
            return country;
        }

        /// <inheritdoc/>
        public async Task<bool> AddNewCountry(Country newCountry)
        {
            var isSuccessful = false;
            if (!CheckCountryExistAsync(newCountry.Name).Result)
            {
                await _context.CountryContexts.AddAsync(newCountry);
                await _context.SaveChangesAsync();
                isSuccessful = true;
            }
            return isSuccessful;
        }

        /// <inheritdoc/>
        public async Task<bool> AddNewRegion(Regions newRegion, string countryName)
        {
            var isSuccessful = false;
            if (CheckCountryExistAsync(countryName).Result)
            {
                var country = await GetCountriesByNameAsync(countryName);

                //Check Region isn't already in there
                Dictionary<string, Regions> regions = country.OwnedRegions.ToDictionary(r => r.Name, r => r);
                if (!regions.ContainsKey(newRegion.Name))
                {
                    newRegion.CountryId = country.Id;
                    newRegion.Country = country;
                    country.OwnedRegions.Add(newRegion);
                    await _context.SaveChangesAsync();
                    isSuccessful = true;
                }
            }
            return isSuccessful;
        }

        /// <inheritdoc/>
        public async Task<List<Regions>> GetRegions(string countryName)
        {
            var country = await GetCountriesByNameAsync(countryName);
            return country.OwnedRegions;
        }

        /// <inheritdoc/>
        public async Task<bool> CheckCountryExistAsync(string name)
        {
            var country = await _context.CountryContexts.Select(c => new Country
            {
                Name = c.Name
            }).AsNoTracking().FirstOrDefaultAsync(c => name.Equals(c.Name));

            return country != null;
        }

        public void CacheResults(List<Country> countries, string cacheKey)
        {
            _cache.Set(cacheKey, countries, TimeSpan.FromHours(1));
        }
    }
}
