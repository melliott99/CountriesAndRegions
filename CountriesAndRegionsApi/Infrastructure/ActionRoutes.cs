namespace CountriesAndRegionsApi.Infrastructure
{
    public static class ActionRoutes
    {
        public const string Empty = "";
        public const string Countries = "countries";
        public const string ByName = Countries + "/{name}";
        public const string CreateCountry = Countries + "/CreateCountry";
        public const string CreateRegion = Countries + "/CreateRegion";
    }
}
