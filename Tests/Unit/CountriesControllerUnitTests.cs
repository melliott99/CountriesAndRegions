using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoFixture;

namespace Tests.Unit
{
    public class CountriesControllerUnitTests
    {

        [Fact]
        public async Task TestThatGetCountriesAsyncReturnsListOfCountries()
        {
            var countries = AutoFixture.Create<IEnumerable<Country>>();
            



        }

    }
}
