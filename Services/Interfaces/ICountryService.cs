using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    /// <summary>
    /// Country Service Interface
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <returns>List of countries</returns>
        Task<List<Country>> GetCountriesAsync();

        /// <summary>
        /// Gets a country the user specified
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Country Object</returns>
        Task<Country> GetCountriesByNameAsync(string name);

        /// <summary>
        /// Checks if the country already exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true or false</returns>
        Task<bool> CheckCountryExistAsync(string name);

        /// <summary>
        /// Adds a new country
        /// </summary>
        /// <param name="newCountry"></param>
        /// <returns></returns>
        Task<bool> AddNewCountry(Country newCountry);

        /// <summary>
        /// Adds a new region to an existing country
        /// </summary>
        /// <param name="newRegion"></param>
        /// <returns></returns>
        Task<bool> AddNewRegion(Regions newRegion, string countryName);

        /// <summary>
        /// Returns all regions belonging to a country
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        Task<List<Regions>> GetRegions(string countryName);
    }
}
