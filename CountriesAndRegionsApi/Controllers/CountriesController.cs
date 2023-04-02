using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Repository.ModelContext;
using CountriesAndRegionsApi.Infrastructure;
using Services;
using Application.Services.Interfaces;
using System.Diagnostics.Metrics;

namespace CountriesAndRegionsApi.Controllers
{
    [ApiController]
    [Route(ActionRoutes.Countries)]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountriesController> _logger;  

        public CountriesController(ICountryService countryService, ILogger<CountriesController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        // GET: Countries
        [HttpGet]
        [Route(ActionRoutes.Empty)]
        [ProducesResponseType(typeof(List<Country>), 200)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetCountries(int? pageSize, int? pageNumber)
        {
            List<Country> response = null;
            if (pageSize != null && pageNumber != null)
            {
                response = await _countryService.GetCountriesAsync((int)pageSize!, (int)pageNumber!);
            }
            else
            {
                response = await _countryService.GetCountriesAsync();
            }
            if (response == null || !response!.Any())
            {
                return NoContent();
            }
            return Ok(response);
        }

        //// GET: Countries/{name}
        [HttpGet]
        [Route(ActionRoutes.ByName)]
        [ProducesResponseType(typeof(List<Country>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountriesByName(string name)
        {
            var response = await _countryService.GetCountriesByNameAsync(name);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // POST: Countries/CreateCountry
        [HttpPost]
        [Route(ActionRoutes.CreateCountry)]
        [ProducesResponseType(typeof(List<Country>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCountry([Bind("Name,CapitalCity,Lattitude,Longitude,PopulationCount,ShortCode")] Country country)
        {
            if (ModelState.IsValid)
            {
                var isSuccessful = await _countryService.AddNewCountry(country);
                if (isSuccessful)
                {
                    return Ok(await _countryService.GetCountriesByNameAsync(country.Name));
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route(ActionRoutes.CreateRegion)]
        [ProducesResponseType(typeof(List<Country>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRegion([Bind("Name,ShortCode")] Regions region, string countryName)
        {
            if (ModelState.IsValid)
            {
                var isSuccessful = await _countryService.AddNewRegion(region, countryName);
         
                if (isSuccessful)
                {
                    return Ok(await _countryService.GetRegions(countryName));
                }
            }
            return BadRequest();
        }
    }
}
