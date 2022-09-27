using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KcloudScript.Model
{
    public class AppSettingsEntity
    {
        public string SwaggerUserId { get; set; }
        public string SwaggerPassword { get; set; }
        public int SlidingExpiry { get; set; }
        public int AbsExpiry { get; set; }
    }
}
