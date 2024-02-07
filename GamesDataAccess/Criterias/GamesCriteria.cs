using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesDataAccess.Criterias;

public class GamesCriteria
{
    public DateOnly? PurchaseDateFrom { get; set; }
    public DateOnly? PurchaseDateTo { get; set; }
    public bool? IsVirtual  { get; set; }
    public string? StoreName { get; set; }
    public string? StoreDescription { get; set; }

    public string? PlatformName { get; set; }
    public string? PlatformDescription { get; set; }

    public string? GameName { get; set; }
    public string? GameDescription { get; set; }
    public string? GameTags { get; set;}

    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
}
