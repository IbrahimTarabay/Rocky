using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models
{
    public class Product{

      [Key]
      public int Id { get; set; }
      public string Name { get; set; }
      public string ShortDesc { get; set; }
      public string Description { get; set; }

      [Range(1, int.MaxValue)]
      public double Price { get; set; }
      public string Image { get; set; }

      [Display(Name = "Category Type")]
      public int CategoryId { get; set; }

      [ForeignKey("CategoryId")]
      public virtual Category Category { get; set; }
      //EF automatically adds a mapping between product and category
      //it will also create a category id column which will be the foreign key relation between both tables

      [Display(Name = "Application Type")]
      public int ApplicationTypeId { get; set; }

      [ForeignKey("ApplicationTypeId")]
      public virtual ApplicationType ApplicationType { get; set; }

    }
}
