using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KcloudScript.Model
{
    public class CityList
    {
        public string? city { get; set; }
        public string? state { get; set; }
    }

    public class ProvinceCodeEntity
    {
        public string? resultStatus { get; set; }
        public string? zip5 { get; set; }
        public string? defaultCity { get; set; }
        public string? defaultState { get; set; }
        public string? defaultRecordType { get; set; }
        public List<CityList>? citiesList { get; set; }
    }
}
