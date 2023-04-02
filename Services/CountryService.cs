using Application.Services.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Repository.ModelContext;
using System.Linq;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly CountryContext _context;
        private readonly ILogger<CountryService> _logger;
        private readonly IMemoryCache _cache;

        private const string ALLCOUNTRYKEY = "all_countries";

        public CountryService(CountryContext context, ILogger<CountryService> logger, IMemoryCache cache) 
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        /// <inheritdoc/>
        public async Task<List<Country>> GetCountriesAsync(int pageSize, int pageNumber)
        {
            //Implements Pagination
            var countries = await _context.CountryContexts
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new Country
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
            return countries;
        }

        /// <inheritdoc/>
        public async Task<List<Country>> GetCountriesAsync()
        {
            var cacheKey = ALLCOUNTRYKEY;
            if (_cache.TryGetValue(cacheKey, out List<Country> cachedCountries))
            {
                return cachedCountries;
            }

            var countries = await _context.CountryContexts
                .Select(c => new Country
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
            var cacheKey = $"{name}_cache";
            if (_cache.TryGetValue(cacheKey, out Country cachedCountry))
            {
                return cachedCountry;
            }

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

            CacheResults(country, cacheKey);

            return country;
        }

        /// <inheritdoc/>
        public async Task<bool> AddNewCountry(Country newCountry)
        {
            var cacheKey = $"{newCountry.Name}_cache";
            var isSuccessful = false;
            var exists = await CheckCountryExistAsync(newCountry.Name);
            if (!exists)
            {
                await _context.CountryContexts.AddAsync(newCountry);
                await _context.SaveChangesAsync();
                isSuccessful = true;
            }
            _cache.Remove(ALLCOUNTRYKEY);
            return isSuccessful;
        }

        /// <inheritdoc/>
        public async Task<bool> AddNewRegion(Regions newRegion, string countryName)
        {
            var cacheKey = $"{countryName}_cache";
            var isSuccessful = false;
            if (await CheckCountryExistAsync(countryName))
            {
                var country = await GetCountriesByNameAsync(countryName);

                //Check Region doesn't already exist
                Dictionary<string, Regions> regions = country.OwnedRegions.ToDictionary(r => r.Name, r => r);
                if (!regions.ContainsKey(newRegion.Name))
                {
                    newRegion.CountryId = country.Id;

                    country.OwnedRegions.Add(newRegion);
                    await _context.RegionContext.AddAsync(newRegion); 

                    await _context.SaveChangesAsync();
                    isSuccessful = true;
                }
            }
            _cache.Remove(ALLCOUNTRYKEY);
            _cache.Remove(cacheKey);
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

        public void CacheResults(Country country, string cacheKey)
        {
            _cache.Set(cacheKey, country, TimeSpan.FromHours(1));
        }
    }
}
