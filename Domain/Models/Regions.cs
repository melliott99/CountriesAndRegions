using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    /// <summary>
    /// Region Model Class
    /// </summary>
    [Table("Regions")]
    public class Regions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortCode { get; set; }
        public int CountryId { get; set; }
        [JsonIgnore]
        public Country Country { get; set; }
    }
}