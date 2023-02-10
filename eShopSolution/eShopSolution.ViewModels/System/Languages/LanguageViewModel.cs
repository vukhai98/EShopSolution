using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Languages
{
    public class LanguageViewModel
    {
        public string Id { get; set; }
     
        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }
}
