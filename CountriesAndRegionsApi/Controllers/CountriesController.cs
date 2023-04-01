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
        //[Route(ActionRoutes.Empty)]
        [ProducesResponseType(typeof(List<Country>), 200)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> GetCountries()
        {
            var response = await _countryService.GetCountriesAsync();
            if (response == null || !response!.Any())
            {
                return NoContent();
            }
            return Ok(response);
        }

        //// GET: Countries/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.CountryContexts == null)
        //    {
        //        return NotFound();
        //    }

        //    var country = await _context.CountryContexts
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (country == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(country);
        //}

        //// GET: Countries/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Countries/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,CapitalCity,Lattitude,Longitude,PopulationCount,ShortCode")] Country country)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(country);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(country);
        //}

        //// GET: Countries/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.CountryContexts == null)
        //    {
        //        return NotFound();
        //    }

        //    var country = await _context.CountryContexts.FindAsync(id);
        //    if (country == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(country);
        //}

        //// POST: Countries/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CapitalCity,Lattitude,Longitude,PopulationCount,ShortCode")] Country country)
        //{
        //    if (id != country.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(country);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CountryExists(country.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(country);
        //}

        //// GET: Countries/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.CountryContexts == null)
        //    {
        //        return NotFound();
        //    }

        //    var country = await _context.CountryContexts
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (country == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(country);
        //}

        //// POST: Countries/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.CountryContexts == null)
        //    {
        //        return Problem("Entity set 'CountryContext.CountryContexts'  is null.");
        //    }
        //    var country = await _context.CountryContexts.FindAsync(id);
        //    if (country != null)
        //    {
        //        _context.CountryContexts.Remove(country);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
