using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace Rocky.Models.ViewModels
{
    public class DetailsVM{
        public DetailsVM() {
            Product = new Product();
        }

        public Product Product { get; set; }
        public bool ExistsInCart { get; set; }
        public NumberFormatInfo myNumberFormatInfo { get; set; }
    }
}
