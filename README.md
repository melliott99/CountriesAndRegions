# CountriesAndRegions
# This Solution Has 4 Endpoints
    1. GetCountries: Returns all countries from the database
    2. GetCountryByName: Returns a single country from the database if exists
    3. CreateCountry: Adds a new country into the database
    4. CreateRegion: Adds a region to an existing country and adds it to the database

# Features
    1. Pagination: Pagination is avaiable on the GetCountries, however, there is no caching for this
    2. Caching: Caching exists for GetCountries and GetCountryByName, using an inmemory cache
    3. Docker: Not implemented due to time constraint
    4. Swagger: Is available to test the API or you can use Postman

# Context
    I made this as a database first, however after using EFCore I believe that code first would be better as
    you can use migrations to update the data if any changes occur.

# Sample Data
    1. GetCountries (Pagination): pageSize = 2, pageNumber = 1
    2. GetCountries: No parameters needed
    3. GetCountriesByName: countryName = "Australia"
    4. CreateCountry: {"name": "MichaelLand","capitalCity": "MichaelVille","populationCount": 1,"shortCode": "ME"} (longitude and lattitude are optional)
    5. CreateRegion: {"name": "MichaelState","shortCode": "MS"}

# Prerequisites
    1. Create Database
        In the directory CountriesAndRegions/Extensions there is two SQL build scripts for the tables you will need and sample data
    2. Update Connection String in the appsettings.json
    3. Build and run