using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocky_Models.ViewModels
{
    public class ProductVM{
      public Product Product { get; set; }

      //IEnumerable can be used with a for-each statement
      public IEnumerable<SelectListItem> CategorySelectList { get; set; }
      public IEnumerable<SelectListItem> ApplicationTypeSelectList { get; set; }
    }
}
