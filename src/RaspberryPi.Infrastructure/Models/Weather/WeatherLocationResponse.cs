namespace RaspberryPi.Infrastructure.Models.Weather
{
    public class WeatherLocationResponse
    {
        public int Version { get; init; }
        public string Key { get; init; }
        public string Type { get; init; }
        public int Rank { get; init; }
        public string LocalizedName { get; init; }
        public string EnglishName { get; init; }
        public Region Region { get; init; }
        public Country Country { get; init; }
        public AdministrativeArea AdministrativeArea { get; init; }
        public bool IsAlias { get; init; }
        public ParentCity ParentCity { get; init; }

        //public TimeZone TimeZone { get; init; }
        //public string PrimaryPostalCode { get; init; }
        //public GeoPosition GeoPosition { get; init; }
        //public List<SupplementalAdminArea> SupplementalAdminAreas { get; init; }
        //public List<string> DataSets { get; init; }
    }

    public class Country
    {
        public string ID { get; init; }
        public string LocalizedName { get; init; }
        public string EnglishName { get; init; }
    }

    public class AdministrativeArea
    {
        public string ID { get; init; }
        public string LocalizedName { get; init; }
        public string EnglishName { get; init; }
        public int Level { get; init; }
        public string LocalizedType { get; init; }
        public string EnglishType { get; init; }
        public string CountryID { get; init; }
    }
    public class ParentCity
    {
        public string Key { get; init; }
        public string LocalizedName { get; init; }
        public string EnglishName { get; init; }
    }

    public class SupplementalAdminArea
    {
        public int Level { get; init; }
        public string LocalizedName { get; init; }
        public string EnglishName { get; init; }
    }

    //public class Elevation
    //{
    //    public Metric Metric { get; init; }
    //    public Imperial Imperial { get; init; }
    //}
}