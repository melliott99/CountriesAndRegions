using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// Model Class for Country Obejct
    /// </summary>
    [Table("Countries")]
    public class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; } 
        public string Name { get; set; }
        public string CapitalCity { get; set; }
        public decimal? Lattitude { get; set; }
        public decimal? Longitude { get; set; }
        public int PopulationCount { get; set; }
        public string? ShortCode { get; set; }
        public List<Regions>? OwnedRegions { get; set; }
    }
}
